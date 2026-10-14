using UnityEngine;

public class BrickPowerupController : MonoBehaviour, IPowerupController
{
    [Header("Powerup (leave empty for a plain brick)")]
    public Animator powerupAnimator;
    public BasePowerup powerup;

    [Header("Breaking")]
    public bool breakable = true;        // false for bricks that should never break
    public GameObject debrisPrefab;      // optional shatter effect

    private Animator brickAnimator;
    private bool used = false;
    public bool IsUsed => used;

    // a brick with no powerup can be hit forever; one with a powerup is spent after use
    public bool HasPowerup => powerup != null;

    void Start()
    {
        brickAnimator = GetComponent<Animator>();
    }

    public void ReleasePowerup()
    {
        if (powerup == null) return;              // plain brick: just bounces
        if (used || powerup.hasSpawned) return;

        powerupAnimator.SetTrigger("spawn");
        Disable();
    }

    public void Disable()
    {
        used = true;
        if (brickAnimator != null)
            brickAnimator.SetBool("disabled", true);
    }

    // called when Super Mario hits a brick that has nothing in it
    public void Break()
    {
        if (!breakable || HasPowerup) return;

        if (debrisPrefab != null)
            Instantiate(debrisPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    public void ResetBrick()
    {
        used = false;
        gameObject.SetActive(true);

        if (brickAnimator != null)
            brickAnimator.SetBool("disabled", false);

        if (powerupAnimator != null)
            powerupAnimator.ResetTrigger("spawn");

        if (powerup != null)
            powerup.ResetPowerup();
    }
}