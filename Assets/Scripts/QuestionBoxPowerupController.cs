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
        boxAnimator.Play("Blink", 0, 0f);

        if (powerupAnimator != null)
            powerupAnimator.ResetTrigger("spawn");

        if (powerup != null)
            powerup.ResetPowerup();
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.tag == "Player" && !powerup.hasSpawned)
    //    {
    //        // show disabled sprite
    //        this.GetComponent<Animator>().SetTrigger("spawned");
    //        // spawn the powerup
    //        powerupAnimator.SetTrigger("spawned");
    //    }
    //}

    //// used by animator
    //public void Disable()
    //{
    //    this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    //    transform.localPosition = new Vector3(0, 0, 0);
    //}



}