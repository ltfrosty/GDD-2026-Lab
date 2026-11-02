using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    public GameConstants gameConstants;

    [Header("Events raised by this Goomba")]
    public UnityEvent damagePlayer;
    public UnityEvent<int> incrementScore;
    public UnityEvent bouncePlayer;

    [Header("Scene refs")]
    public Animator goombaAnimator;
    public AudioClip stompSound;

    private Rigidbody2D enemyBody;
    private Collider2D enemyCollider;

    private float originalX;
    private float maxOffset;
    private float enemyPatrolTime;
    private int moveRight = -1;
    private Vector2 velocity;

    private Vector3 startPosition;
    private bool isStomped = false;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

        maxOffset = gameConstants.goombaMaxOffset;
        enemyPatrolTime = gameConstants.goombaPatrolTime;

        startPosition = transform.localPosition;
        originalX = transform.position.x;

        ComputeVelocity();
    }

    void ComputeVelocity()
    {
        velocity = new Vector2(moveRight * maxOffset / enemyPatrolTime, 0);
    }

    void FixedUpdate()
    {
        if (isStomped) return;

        if (Mathf.Abs(enemyBody.position.x - originalX) >= maxOffset)
        {
            moveRight *= -1;
            ComputeVelocity();
        }

        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isStomped) return;

        // our collider is a trigger, so FireMario's fireballs arrive here (not in OnCollisionEnter2D)
        if (other.GetComponent<FireballController>() != null)
        {
            Destroy(other.gameObject);
            Die();
            return;
        }

        if (!other.gameObject.CompareTag("Player")) return;

        // Starman Mario kills us on any touch; damagePlayer still fires so his buff knows it was used
        BuffStateController buff = other.GetComponent<BuffStateController>();
        if (buff != null && buff.isStarmanActive)
        {
            Die();
            damagePlayer.Invoke();
            return;
        }

        // stomp only when Mario comes down onto our head: his feet are above our centre
        // and he is not moving up. Running into us from the side always damages him.
        Rigidbody2D playerBody = other.attachedRigidbody;
        bool falling = playerBody == null || playerBody.linearVelocity.y <= 0.01f;
        bool feetAboveCentre = other.bounds.min.y >= enemyCollider.bounds.center.y;

        if (falling && feetAboveCentre)
            Stomp();
        else
            damagePlayer.Invoke();
    }

    void Stomp()
    {
        bouncePlayer.Invoke();
        Die();
    }

    void Die()
    {
        isStomped = true;

        goombaAnimator.SetTrigger("stomped");

        if (stompSound != null)
            AudioSource.PlayClipAtPoint(stompSound, transform.position);

        enemyCollider.enabled = false;
        incrementScore.Invoke(gameConstants.goombaScoreValue);

        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(gameConstants.goombaDestroyDelay);
        gameObject.SetActive(false);
    }

    public void GameRestart()
    {
        StopAllCoroutines();

        isStomped = false;
        gameObject.SetActive(true);
        enemyCollider.enabled = true;

        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();

        goombaAnimator.ResetTrigger("stomped");
        goombaAnimator.Play("alive", 0, 0f);
    }
}