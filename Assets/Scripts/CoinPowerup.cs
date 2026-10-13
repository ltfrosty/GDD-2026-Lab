using UnityEngine;

public class CoinPowerup : BasePowerup
{
    public AudioClip collectSound;
    public Transform spriteChild;      // the Coin sprite child

    private Vector3 hiddenLocalPosition;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.Coin;

        if (spriteChild != null)
            hiddenLocalPosition = spriteChild.localPosition;

        if (rigidBody != null)
            rigidBody.simulated = false;
    }

    // called by an Animation Event at the start of the pop clip
    public override void SpawnPowerup()
    {
        spawned = true;
    }

    // called by an Animation Event on the frame the coin lands back in the box
    public void OnLanded()
    {
        AudioSource.PlayClipAtPoint(collectSound, transform.position);
        ApplyPowerup(null);
        gameObject.SetActive(false);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        GameManager.instance.IncreaseScore(1);
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();

        gameObject.SetActive(true);

        if (spriteChild != null)
            spriteChild.localPosition = hiddenLocalPosition;
    }
}