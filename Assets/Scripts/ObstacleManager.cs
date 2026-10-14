using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.gameRestart.RemoveListener(GameRestart);
    }

    public void GameRestart()
    {
        foreach (QuestionBoxBounce box in GetComponentsInChildren<QuestionBoxBounce>(true))
            box.ResetBox();

        foreach (BrickBounce brick in GetComponentsInChildren<BrickBounce>(true))
            brick.ResetBrick(); 
    }
}