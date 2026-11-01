using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPowerupApplicable
{
    public GameConstants gameConstants;

    float speed, maxSpeed, upSpeed, deathImpulse, bounceOffGoombaForce;
    private bool faceRightState = true;
    private bool onGroundState = true;
    private bool jumpedState = false;
    private bool moving = false;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioClip marioDeathClip;

    [Header("Events this script raises")]
    public SimpleGameEvent onGameOver;

    public BoolVariable marioFaceRight;
    [System.NonSerialized] public bool alive = true;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7) | (1 << 10);

    void Start()
    {
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        upSpeed = gameConstants.upSpeed;
        deathImpulse = gameConstants.deathImpulse;
        bounceOffGoombaForce = gameConstants.bounceOffGoombaForce;

        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);
        updateMarioShouldFaceRight(faceRightState);
    }

    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));
    }

    void FixedUpdate()
    {
        if (alive && moving)
            Move(faceRightState ? 1 : -1);
    }

    // ---- input callbacks (wired via ActionManager) ----

    public void Jump()
    {
        if (alive && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }

    public void MoveCheck(int value)
    {

        if (!alive) return;

        if (value == 0) { moving = false; return; }

        FlipMarioSprite(value);
        moving = true;
        Move(value);
    }

    void Move(int value)
    {
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(new Vector2(value, 0) * speed);
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            updateMarioShouldFaceRight(false);
            marioSprite.flipX = !faceRightState;

            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
        else if (value == 1 && !faceRightState)
        {
            updateMarioShouldFaceRight(true);
            marioSprite.flipX = !faceRightState;

            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    private void updateMarioShouldFaceRight(bool value)
    {
        faceRightState = value;
        marioFaceRight.SetValue(faceRightState);
    }

    // ---- ground detection ----

    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) && !onGroundState)
        {
            onGroundState = true;
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // ---- event callbacks (public, wired via GameEventListener) ----

    // OnDamagePlayer
    public void DamageMario()
    {
        GetComponent<MarioStateController>()
            .SetPowerup(PowerupType.Damage);
    }
    // public void DamageMario()
    // {
    //     if (!alive) return;
    //     alive = false;

    //     marioAnimator.Play("mario-die");

    //     if (marioDeathClip != null)
    //         AudioSource.PlayClipAtPoint(marioDeathClip, transform.position);

    //     PlayDeathImpulse();
    //     StartCoroutine(DelayedGameOver());
    // }

    // OnBouncePlayer
    public void BounceOffEnemy()
    {
        if (!alive) return;

        marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, 0f);
        marioBody.AddForce(Vector2.up * bounceOffGoombaForce, ForceMode2D.Impulse);
    }

    // OnPowerupAffectsPlayer
    public void RequestPowerupEffect(IPowerup i)
    {
        i.ApplyPowerup(this);
    }

    public void MakeSuperMario()
    {
        // TODO: scale up, swap animator layer, grant one free hit
    }

    public void MakeInvincible()
    {
        // TODO: timed invincibility, flashing sprite
    }

    // OnGameRestart
    public void GameRestart()
    {
        StopAllCoroutines();

        marioBody.transform.position = gameConstants.marioStartPosition;
        marioBody.linearVelocity = Vector2.zero;

        //faceRightState = true;
        updateMarioShouldFaceRight(true);
        marioSprite.flipX = false;
        moving = false;
        jumpedState = false;
        onGroundState = true;

        marioAnimator.SetTrigger("gameRestart");
        marioAnimator.SetBool("onGround", onGroundState);

        alive = true;
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSecondsRealtime(gameConstants.gameOverDelay);
        onGameOver.Raise();
    }

    void PlayJumpSound() => marioAudio.PlayOneShot(marioAudio.clip);
    //void PlayDeathImpulse() => marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    public void PlayDeathImpulse()
    {
        alive = false;
        moving = false;
        marioBody.linearVelocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);

        if (marioDeathClip != null)
            marioAudio.PlayOneShot(marioDeathClip);
    }

    public void GameOverScene()
    {
        onGameOver.Raise();
    }
}