using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;

    private int score = 0;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.activeSceneChanged += SceneSetup;
    }
    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();   // new scene's HUD needs initialising
        SetScore(score);      // carry the score over
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        GoombaStomp.GoombaStomped += HandleGoombaStomped;
    }

    void OnDisable()
    {
        GoombaStomp.GoombaStomped -= HandleGoombaStomped;
    }

    private void HandleGoombaStomped(int amount)
    {
        IncreaseScore(amount);
    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }


    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}