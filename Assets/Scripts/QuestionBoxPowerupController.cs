using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    public Animator powerupAnimator;
    public BasePowerup powerup; // reference to this question box's powerup

    private Animator boxAnimator;
    private bool used = false;
    public bool IsUsed => used;

    void Start()
    {
        boxAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReleasePowerup()
    {
        if (used || powerup == null || powerup.hasSpawned) return;

        powerupAnimator.SetTrigger("spawn");   // its end-of-clip event calls SpawnPowerup
        Disable();
    }

    public void Disable()
    {
        used = true;
        boxAnimator.SetBool("disabled", true);
    }

    public void ResetBox()
    {
        used = false;
        boxAnimator.ResetTrigger("spawn");
        boxAnimator.SetBool("disabled", false);
        boxAnimator.Play("blink", 0, 0f);

        if (powerupAnimator != null)
            powerupAnimator.ResetTrigger("spawn");

        if (powerup != null)
            powerup.ResetPowerup();
    }


}