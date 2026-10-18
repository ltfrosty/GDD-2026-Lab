using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntVariable gameScore;
    public GameConstants gameConstants;

    [Header("Events this script raises")]
    public SimpleGameEvent onGameStart;
    public SimpleGameEvent onUpdateScore;

    void Start()
    {
        Application.targetFrameRate = gameConstants.targetFrameRate;
        Time.timeScale = 1.0f;
        onGameStart.Raise();
        onUpdateScore.Raise();
    }

    // OnIncrementScore (int payload)
    public void IncreaseScore(int amount)
    {
        gameScore.ApplyChange(amount);
        onUpdateScore.Raise();
    }

    // OnGameResumed
    public void ResumeGame() => Time.timeScale = 1.0f;

    // OnGameOver
    public void GameOver() => Time.timeScale = 0.0f;

    // OnGameRestart
    public void GameRestart()
    {
        gameScore.Value = 0;
        onUpdateScore.Raise();
        Time.timeScale = 1.0f;
    }

    // OnPowerupAffectsManager (IPowerup payload)
    public void RequestPowerupEffect(IPowerup i)
    {
        i.ApplyPowerup(this);
    }
}