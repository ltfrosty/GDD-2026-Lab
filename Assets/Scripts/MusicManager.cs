using UnityEngine;

public class MusicManager : Singleton<MusicManager>
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