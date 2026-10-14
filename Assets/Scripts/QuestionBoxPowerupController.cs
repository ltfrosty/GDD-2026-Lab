using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup;
    public string powerupHiddenState = "idle";

    private Animator boxAnimator;
    private bool used = false;
    public bool IsUsed => used;

    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }

    public void ReleasePowerup()
    {
        if (used || powerup == null || powerup.hasSpawned) return;

        powerupAnimator.SetTrigger("spawn");
        Disable();
    }

    public void Disable()
    {
        used = true;
        if (boxAnimator != null)
            boxAnimator.SetBool("disabled", true);
    }

    public void ResetBox()
    {
        used = false;

        if (boxAnimator != null)
        {
            boxAnimator.SetBool("disabled", false);
            boxAnimator.Play("blink", 0, 0f);
        }

        // reactivate FIRST — animator calls on an inactive object are ignored
        if (powerup != null)
            powerup.ResetPowerup();

        if (powerupAnimator != null)
        {
            powerupAnimator.ResetTrigger("spawn");
            powerupAnimator.Play(powerupHiddenState, 0, 0f);
        }
    }
}