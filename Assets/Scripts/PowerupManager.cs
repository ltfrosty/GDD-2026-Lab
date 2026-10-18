using UnityEngine;
using UnityEngine.Events;

public class PowerupManager : MonoBehaviour
{
    [Header("Events this script raises")]
    public PowerupGameEvent onPowerupAffectsPlayer;
    public PowerupGameEvent onPowerupAffectsManager;

    // OnPowerupCollected (IPowerup payload)
    public void FilterAndCastPowerup(IPowerup i)
    {
        switch (i.powerupType)
        {
            case PowerupType.MagicMushroom:
            case PowerupType.StarMan:
                onPowerupAffectsPlayer.Raise(i);
                break;

            case PowerupType.Coin:
            case PowerupType.OneUpMushroom:
                onPowerupAffectsManager.Raise(i);
                break;
        }
    }
}