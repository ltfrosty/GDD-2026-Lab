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

    [Header("Stomp detection")]
    public LayerMask playerLayerMask;
    public Vector2 stompCheckSize = new Vector2(0.7f, 0.2f);
    public float stompCheckDistance = 0.5f;

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
        if (!other.gameObject.CompareTag("Player")) return;

        // cast UP from our own position — is the player standing on our head?
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position, stompCheckSize, 0f,
            Vector2.up, stompCheckDistance, playerLayerMask);

        bool stomped = hit.collider != null
                       && hit.collider.gameObject == other.gameObject;

        if (stomped)
            Stomp();
        else
            damagePlayer.Invoke();
    }

    void Stomp()
    {
        isStomped = true;

        goombaAnimator.SetTrigger("stomped");

        if (stompSound != null)
            AudioSource.PlayClipAtPoint(stompSound, transform.position);

        bouncePlayer.Invoke();

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + Vector3.up * stompCheckDistance, stompCheckSize);
    }
}