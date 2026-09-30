using UnityEngine;
using System.Collections;

public class QuestionBoxBounce : MonoBehaviour
{
    private Rigidbody2D boxBody;
    private RigidbodyConstraints2D restingConstraints;
    private bool isDisabled = false;

    public float bounceDisplacement = 0.3f;
    public float maxBounceTime = 1.5f;

    public GameObject coinPrefab;
    public AudioSource boxAudio;
    public AudioClip coinSound;

    public SpriteRenderer boxSprite;
    public Animator boxAnimator;

    public void ResetBox()
    {
        isDisabled = false;
        boxAnimator.SetBool("disabled", false);
    }
    void SpawnCoin()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        boxAudio.PlayOneShot(coinSound);
    }
    

    void DisableBox()
    {
        isDisabled = true;
        boxAnimator.SetBool("disabled", true);   // transitions out of the blink loop
    }

    void Start()
    {
        boxBody = GetComponent<Rigidbody2D>();

        boxBody.sleepMode = RigidbodySleepMode2D.NeverSleep;

        // Preserve whatever's already set in the Inspector (e.g. FreezePositionX),
        // and add FreezePositionY on top so standing on top does nothing by default.
        restingConstraints = boxBody.constraints | RigidbodyConstraints2D.FreezePositionY;
        boxBody.constraints = restingConstraints;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDisabled) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Upward normal = box's underside got hit (Mario's head hit it from below)
            if (contact.normal.y > 0.5f)
            {
                StartCoroutine(Bounce());
                break;
            }
        }
    }

    IEnumerator Bounce()
    {
        boxBody.constraints = restingConstraints & ~RigidbodyConstraints2D.FreezePositionY;
        Vector2 restPosition = boxBody.position;
        boxBody.position += Vector2.up * bounceDisplacement;
        boxBody.WakeUp();

        SpawnCoin();

        float elapsed = 0f;
        do
        {
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while (elapsed < maxBounceTime && Vector2.Distance(boxBody.position, restPosition) > 0.02f);

        boxBody.position = restPosition;   // snap cleanly to rest, avoiding tiny leftover offset
        boxBody.linearVelocity = Vector2.zero;
        boxBody.constraints = restingConstraints;

        DisableBox();
    }

   
}