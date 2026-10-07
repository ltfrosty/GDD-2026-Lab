using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Rendering;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerMovement : MonoBehaviour
{
    // Mario movement variables
    public float speed = 100;
    public float maxSpeed = 15;
    public float upSpeed = 50;
    public float deathImpulse = 40;
    private bool faceRightState = true;
    private bool onGroundState = true;

    // Mario Unity things
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;
    // public AudioClip marioDeath;

    public GameObject enemies;
    public GameObject questionBoxes;

    public Transform gameCamera;
    public GameManager gameManager;

    // state
    [System.NonSerialized]
    public bool alive = true;



    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        //gameOverPanel.SetActive(false);

        marioBody = GetComponent<Rigidbody2D>();
        // "GetComponent<Rigidbody2D>() searches the GameObject this script is attached to, finds the
        // you then component of type Rigidbody2D on it, and returns a reference to it, which store
        // in the marioBody variable. Without that line, marioBody would just be null forever, even if the
        // GameObject has a Rigidbody2D sitting right there in the Inspector — Unity wouldn't connect them for you"
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator.SetBool("onGround", onGroundState);

    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));

    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    private bool jumpedState = false;

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }
    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }


    private bool moving = false;
    // FixedUpdate is called 50 times a second      
    void FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (alive)
        {
            // collide with goomba -> game over
            if (other.gameObject.CompareTag("Enemy"))
            {
                // play death animation
                marioAnimator.Play("mario-die");
                // marioAudio.PlayOneShot(marioDeath);
                // changed to use AudioSource component instead of AudioClip directly
                marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
                PlayDeathImpulse();

                alive = false;

                StartCoroutine(DelayedGameOver());

            }
        }

    }

    IEnumerator DelayedGameOver()
    {
        // let physics actually run for a moment so the impulse visibly plays out

        yield return new WaitForSecondsRealtime(1.2f);
        // Frame 1: coroutine starts, hits yield return, control returns to Unity.
        // Frames 2 through ~36(at 60fps over 0.6s): Unity keeps running everything else completely normally — Mario's death animation plays, physics integrates his velocity into visible upward motion, gravity arcs him back down, etc.
        // After 0.6 real seconds have elapsed, Unity resumes the coroutine right after the yield line, running gameManager.GameOver().

        gameManager.GameOver();
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-10.0f, -4.69f, 0.0f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset question boxes - idk where else to put this
        foreach (Transform eachChild in questionBoxes.transform)
        {
            QuestionBoxBounce box = eachChild.GetComponentInChildren<QuestionBoxBounce>();
            if (box != null)
            {
                box.ResetBox();
            }
        }

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 0, -10);
    }

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }



}