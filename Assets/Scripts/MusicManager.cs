using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public AudioSource themeMusic;

    void Start()
    {
        GameManager.instance.gameOver.AddListener(StopMusic);
        GameManager.instance.gameRestart.AddListener(RestartMusic);

        if (!themeMusic.isPlaying)
            themeMusic.Play();
    }

    void StopMusic()
    {
        themeMusic.Stop();
    }

    void RestartMusic()
    {
        themeMusic.Stop();
        themeMusic.Play();
    }
}