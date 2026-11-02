using UnityEngine;

public class FireFlowerPowerup : BasePowerup
{
    public GameConstants gameConstants;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.FireFlower;

        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned) return;

        if (col.gameObject.CompareTag("Player"))
        {
            Collect();
        }
    }

    // Animation Event at the END of the emerge clip
    // unlike the mushroom, the flower does not move: it just sits on top of the box
    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        MarioStateController mario;

        if (i.TryGetComponent<MarioStateController>(out mario))
        {
            mario.SetPowerup(this.powerupType);
        }
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}
