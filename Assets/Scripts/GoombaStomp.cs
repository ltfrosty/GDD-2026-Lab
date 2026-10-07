using System;
using System.Collections;
using UnityEngine;

public class GoombaStomp : MonoBehaviour
{
    // static C# event — any Goomba can raise it, anyone can subscribe without a direct reference
    public static event Action<int> GoombaStomped;

    public Animator goombaAnimator;
    public int scoreValue = 1;
    public float destroyDelay = 0.5f;

    private bool isStomped = false;
    public bool IsStomped => isStomped;

    public void Stomp()
    {
        if (isStomped) return;
        isStomped = true;

        goombaAnimator.SetTrigger("stomped");
        GetComponent<Collider2D>().enabled = false;
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null) movement.enabled = false;

        GoombaStomped?.Invoke(scoreValue);

        StartCoroutine(DeactivateAfterDelay());
    }

    IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        gameObject.SetActive(false);   // hide instead of destroy
    }

    public void ResetGoomba()
    {
        isStomped = false;
        gameObject.SetActive(true);

        GetComponent<Collider2D>().enabled = true;
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null) movement.enabled = true;

        goombaAnimator.ResetTrigger("stomped");   
        goombaAnimator.Play("alive", 0, 0f);      

        movement?.GameRestart();

    }

}