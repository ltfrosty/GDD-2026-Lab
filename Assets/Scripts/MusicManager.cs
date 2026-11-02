using UnityEngine;

// no longer a Singleton: each scene has its own MusicManager (no DontDestroyOnLoad)
public class MusicManager : MonoBehaviour
{
    public AudioSource themeMusic;

    void Start()
    {
        if (!themeMusic.isPlaying)
            themeMusic.Play();
    }

    // OnGameOver
    public void StopMusic()
    {
        themeMusic.Stop();
    }

    // OnGameRestart
    public void RestartMusic()
    {
        themeMusic.Stop();
        themeMusic.Play();
    }
}
