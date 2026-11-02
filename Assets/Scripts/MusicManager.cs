using UnityEngine;

// no longer a Singleton: each scene has its own MusicManager (no DontDestroyOnLoad)
public class MusicManager : MonoBehaviour
{
    public AudioSource themeMusic;
    public AudioClip starmanMusic;

    private AudioSource starmanSource;

    void Start()
    {
        // second source so the theme can be paused and resumed where it left off
        starmanSource = gameObject.AddComponent<AudioSource>();
        starmanSource.clip = starmanMusic;
        starmanSource.loop = true;
        starmanSource.playOnAwake = false;
        starmanSource.outputAudioMixerGroup = themeMusic.outputAudioMixerGroup;

        if (!themeMusic.isPlaying)
            themeMusic.Play();
    }

    // OnGameOver
    public void StopMusic()
    {
        starmanSource.Stop();
        themeMusic.Stop();
    }

    // OnGameRestart
    public void RestartMusic()
    {
        starmanSource.Stop();
        themeMusic.Stop();
        themeMusic.Play();
    }

    // OnStarmanStart
    public void StartStarmanMusic()
    {
        themeMusic.Pause();
        starmanSource.Play();
    }

    // OnStarmanEnd
    public void StopStarmanMusic()
    {
        if (!starmanSource.isPlaying) return;
        starmanSource.Stop();
        themeMusic.UnPause();
    }
}
