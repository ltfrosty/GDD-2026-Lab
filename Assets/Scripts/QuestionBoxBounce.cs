using UnityEngine;
using System.Collections;

public class QuestionBoxBounce : MonoBehaviour
{
    private Rigidbody2D boxBody;
    private RigidbodyConstraints2D restingConstraints;
    private QuestionBoxPowerupController powerupController;
    private bool isDisabled = false;

    public float bounceDisplacement = 0.3f;
    public float maxBounceTime = 1.5f;

    void Start()
    {
        boxBody = GetComponent<Rigidbody2D>();
        boxBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        restingConstraints = boxBody.constraints | RigidbodyConstraints2D.FreezePositionY;
        boxBody.constraints = restingConstraints;

        powerupController = GetComponent<QuestionBoxPowerupController>();
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

        if (powerupController != null)
            powerupController.ReleasePowerup();

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

    }

    public void ResetBox()
    {
        StopCoroutine(nameof(Bounce));
        boxBody.linearVelocity = Vector2.zero;
        boxBody.constraints = restingConstraints;

        if (powerupController != null)
            powerupController.ResetBox();
    }


}