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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        // other instructions
        // subscribe to events
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameOver.AddListener(GameOver);
        GameManager.instance.gameRestart.AddListener(GameStart);
        GameManager.instance.scoreChange.AddListener(SetScore);
    }

    void OnDestroy()
    {
        if (GameManager.instance == null) return;
        GameManager.instance.gameStart.RemoveListener(GameStart);
        GameManager.instance.gameOver.RemoveListener(GameOver);
        GameManager.instance.gameRestart.RemoveListener(GameStart);
        GameManager.instance.scoreChange.RemoveListener(SetScore);
    }

    public void GameStart()
    {
        // hide gameover panel
        gameOverPanel.SetActive(false);
        scoreText.transform.localPosition = scoreTextPosition[0];
        restartButton.localPosition = restartButtonPosition[0];
    }

    public void SetScore(int score)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }


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
