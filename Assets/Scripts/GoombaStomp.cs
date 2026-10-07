using System;
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

        // stop it from moving/colliding further once squished
        GetComponent<Collider2D>().enabled = false;
        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null) movement.enabled = false;

        GoombaStomped?.Invoke(scoreValue);   // broadcast to anyone listening

        Destroy(gameObject, destroyDelay);
    }
}