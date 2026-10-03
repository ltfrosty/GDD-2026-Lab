using UnityEngine;

public class Coin : MonoBehaviour
{
    public AudioClip collectSound;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // called via an animation event on the frame where the coin visually lands inside the box
    public void OnLanded()
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        gameManager.IncreaseScore(1);
    }

    // called via an animation event on the final frame of the clip (or can be the same frame as OnLanded, or shortly after)
    public void OnPopFinished()
    {
        Destroy(gameObject);
    }
}