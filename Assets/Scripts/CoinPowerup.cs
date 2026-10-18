using UnityEngine;

public class CoinPowerup : BasePowerup
{
    public AudioClip collectSound;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.Coin;

        if (rigidBody != null) rigidBody.simulated = false;
    }

    // Animation Event at the START of the pop clip
    public override void SpawnPowerup() => spawned = true;

    // Animation Event on the frame the coin lands back in the box
    public void OnLanded()
    {
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        Collect();
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        GameManager manager = i as GameManager;
        if (manager != null) manager.IncreaseScore(1);
    }
}