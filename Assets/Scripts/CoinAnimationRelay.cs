using UnityEngine;

public class CoinAnimationRelay : MonoBehaviour
{
    public CoinPowerup powerup;

    public void OnLanded()
    {
        powerup.OnLanded();
    }
}