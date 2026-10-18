using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePowerupController : MonoBehaviour, IPowerupController
{
    public GameConstants gameConstants;

    [Header("Powerup (leave empty for a plain box)")]
    public Animator powerupAnimator;
    public BasePowerup powerup;

    [Tooltip("Name of the powerup animator's pre-emerge state")]
    public string powerupHiddenState = "idle";

    [Header("Audio")]
    public AudioClip bumpSound;

    protected Rigidbody2D boxBody;
    protected RigidbodyConstraints2D restingConstraints;
    protected Animator boxAnimator;
    protected bool used = false;
    protected bool isBouncing = false;

    public bool IsUsed => used;
    public bool HasPowerup => powerup != null;

    protected virtual void Start()
    {
        boxBody = GetComponent<Rigidbody2D>();
        boxAnimator = GetComponent<Animator>();

        boxBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        restingConstraints = boxBody.constraints | RigidbodyConstraints2D.FreezePositionY;
        boxBody.constraints = restingConstraints;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBouncing || !CanBeHit()) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                StartCoroutine(Bounce());
                break;
            }
        }
    }

    // question box: dead once used. brick: can always be bumped.
    protected abstract bool CanBeHit();

    protected IEnumerator Bounce()
    {
        Debug.Log("Bounce()");
        isBouncing = true;

        boxBody.constraints = restingConstraints & ~RigidbodyConstraints2D.FreezePositionY;
        Vector2 restPosition = boxBody.position;
        boxBody.position += Vector2.up * gameConstants.boxBounceDisplacement;
        boxBody.WakeUp();

        if (bumpSound != null)
            AudioSource.PlayClipAtPoint(bumpSound, transform.position);

        ReleasePowerup();

        float elapsed = 0f;
        do
        {
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while (elapsed < gameConstants.boxMaxBounceTime
               && Vector2.Distance(boxBody.position, restPosition) > 0.02f);

        boxBody.position = restPosition;
        boxBody.linearVelocity = Vector2.zero;
        boxBody.constraints = restingConstraints;

        isBouncing = false;
    }

    protected void ReleasePowerup()
    {
        if (used || powerup == null || powerup.hasSpawned) return;


        powerupAnimator.SetTrigger("spawn");
        Disable();
    }

    public virtual void Disable()
    {
        used = true;
        if (boxAnimator != null)
            boxAnimator.SetBool("disabled", true);
    }

    // OnGameRestart
    public virtual void GameRestart()
    {
        StopAllCoroutines();
        isBouncing = false;
        used = false;

        gameObject.SetActive(true);

        boxBody.linearVelocity = Vector2.zero;
        boxBody.constraints = restingConstraints;

        if (boxAnimator != null)
            boxAnimator.SetBool("disabled", false);

        // reactivate the powerup FIRST — animator calls on an inactive object are ignored
        if (powerup != null)
            powerup.ResetPowerup();

        if (powerupAnimator != null)
        {
            powerupAnimator.ResetTrigger("spawn");
            powerupAnimator.Play(powerupHiddenState, 0, 0f);
        }
    }
}