using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip collectSound;
    private bool collected = false;

    public void OnPopFinished()
    {
        if (!collected)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.gameObject.CompareTag("Player")) return;

        collected = true;
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        // TODO: increase score here once scoring is wired up

        Destroy(gameObject);
    }
}