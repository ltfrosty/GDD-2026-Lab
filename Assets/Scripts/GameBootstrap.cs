using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    public GameConstants gameConstants;

    void Awake()
    {
        Application.targetFrameRate = gameConstants.targetFrameRate;
    }
}