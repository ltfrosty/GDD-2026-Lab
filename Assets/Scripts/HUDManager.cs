using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HUDManager : MonoBehaviour
{
    private Vector3[] scoreTextPosition = {
        new Vector3(-700, 473, 0),
        new Vector3(40, 40, 0)
        };
    private Vector3[] restartButtonPosition = {
        new Vector3(880, 470, 0),
        new Vector3(0, -115, 0)
    };

    public GameObject scoreText;
    public Transform restartButton;
    public GameObject gameOverPanel;
    public GameObject highscoreText;

    public IntVariable gameScore;


    // OnGameStart, OnGameRestart
    public void GameStart()
    {
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
        SetScore();
    }

    // OnUpdateScore
    public void SetScore()
    {

        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + gameScore.Value;
    }

    // OnGameOver
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        scoreText.transform.localPosition = scoreTextPosition[1];
        restartButton.localPosition = restartButtonPosition[1];

        // set highscore
        highscoreText.GetComponent<TextMeshProUGUI>().text = "TOP-" + gameScore.previousHighestValue.ToString("D6");
        // show
        highscoreText.SetActive(true);
    }
}
