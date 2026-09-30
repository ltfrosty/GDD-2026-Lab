using UnityEngine;
using System.Collections;

public class BrickBounce : MonoBehaviour
{
    private Rigidbody2D brickBody;
    private RigidbodyConstraints2D restingConstraints;
    private bool isBouncing = false;   // prevents a second bounce while one is still playing

    public float bounceDisplacement = 0.3f;
    public float maxBounceTime = 1.5f;

    [Header("Coin Variant")]
    public bool spawnsCoin = false;   // toggle per-prefab-variant
    public GameObject coinPrefab;

    void Start()
    {
        brickBody = GetComponent<Rigidbody2D>();
        brickBody.sleepMode = RigidbodySleepMode2D.NeverSleep;

        restingConstraints = brickBody.constraints | RigidbodyConstraints2D.FreezePositionY;
        brickBody.constraints = restingConstraints;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBouncing) return;                                  // ignore hits mid-bounce
        if (!collision.gameObject.CompareTag("Player")) return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // Only bounce if hit from directly below — same check as the question box
            if (contact.normal.y > 0.5f)
            {
                StartCoroutine(Bounce());
                break;
            }
        }
    }

    IEnumerator Bounce()
    {
        isBouncing = true;

        brickBody.constraints = restingConstraints & ~RigidbodyConstraints2D.FreezePositionY;
        Vector2 restPosition = brickBody.position;
        brickBody.position += Vector2.up * bounceDisplacement;
        brickBody.WakeUp();

        if (spawnsCoin)
        {
            SpawnCoin();
        }

        float elapsed = 0f;
        do
        {
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        while (elapsed < maxBounceTime && Vector2.Distance(brickBody.position, restPosition) > 0.02f);

        brickBody.position = restPosition;
        brickBody.linearVelocity = Vector2.zero;
        brickBody.constraints = restingConstraints;

        isBouncing = false;   // allow the next hit to bounce again
    }

    void SpawnCoin()
    {
        Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
        Instantiate(coinPrefab, spawnPos, Quaternion.identity);
    }
}