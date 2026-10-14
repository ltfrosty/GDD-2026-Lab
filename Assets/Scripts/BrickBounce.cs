using UnityEngine;
using System.Collections;

public class BrickBounce : MonoBehaviour
{
    public float bounceDisplacement = 0.3f;
    public float maxBounceTime = 1f;

    private Rigidbody2D brickBody;
    private RigidbodyConstraints2D restingConstraints;
    private BrickPowerupController controller;
    private bool isBouncing = false;

    void Start()
    {
        brickBody = GetComponent<Rigidbody2D>();
        brickBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        restingConstraints = brickBody.constraints | RigidbodyConstraints2D.FreezePositionY;
        brickBody.constraints = restingConstraints;

        controller = GetComponent<BrickPowerupController>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBouncing) return;
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

    IEnumerator Bounce()
    {
        isBouncing = true;

        brickBody.constraints = restingConstraints & ~RigidbodyConstraints2D.FreezePositionY;
        Vector2 restPosition = brickBody.position;
        brickBody.position += Vector2.up * bounceDisplacement;
        brickBody.WakeUp();

        if (controller != null)
            controller.ReleasePowerup();

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

        isBouncing = false;
    }

    public void ResetBrick()
    {
        StopCoroutine(nameof(Bounce));
        isBouncing = false;
        brickBody.linearVelocity = Vector2.zero;
        brickBody.constraints = restingConstraints;

        if (controller != null)
            controller.ResetBrick();
    }
}