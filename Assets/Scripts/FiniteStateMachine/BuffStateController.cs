using System.Collections;
using UnityEngine;

// second FSM on Mario that only manages buffs (Starman), separate from MarioStateController
public class BuffStateController : StateController
{
    public PowerupType currentBuffType = PowerupType.Default;
    public GameConstants gameConstants;

    [System.NonSerialized] public bool isStarmanActive = false;

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    public override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        base.Start();
        GameRestart(); // clear buff in the beginning, go to start state
    }

    // this should be added to the GameRestart EventListener as callback
    public void GameRestart()
    {
        // clear buff
        currentBuffType = PowerupType.Default;
        // set the start state (exit actions of current state clean up any active buff)
        TransitionToState(startState);
    }

    public void SetBuff(PowerupType i)
    {
        currentBuffType = i;
    }

    // called by StartStarmanAction
    public void StartStarman(AudioClip startClip)
    {
        isStarmanActive = true;
        if (startClip != null) audioSource.PlayOneShot(startClip);
        StartCoroutine(CycleSpriteColor());
    }

    // called by ExitStarmanAction
    public void ExitStarman(AudioClip exitClip)
    {
        isStarmanActive = false;
        if (exitClip != null) audioSource.PlayOneShot(exitClip);
    }

    private IEnumerator CycleSpriteColor()
    {
        Color[] colors = { Color.white, Color.yellow, Color.red, Color.green };
        int i = 0;
        while (isStarmanActive)
        {
            spriteRenderer.color = colors[i++ % colors.Length];
            yield return new WaitForSeconds(gameConstants.flickerInterval);
        }

        spriteRenderer.color = Color.white;
    }
}
