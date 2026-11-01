using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    public GameConstants gameConstants;
    public LayerMask flipOnLayers;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.MagicMushroom;

        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned) return;

        if (col.gameObject.CompareTag("Player"))
        {
            Collect();
            return;
        }

        ContactPoint2D contact = col.GetContact(0);

        if ((flipOnLayers.value & (1 << col.gameObject.layer)) != 0
            && Mathf.Abs(contact.normal.x) > 0.5f)
        {
            goRight = !goRight;
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
            rigidBody.AddForce(
                Vector2.right * gameConstants.powerupMoveForce * (goRight ? 1 : -1),
                ForceMode2D.Impulse);
        }
    }

    // Animation Event at the END of the emerge clip
    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(
            Vector2.right * gameConstants.powerupMoveForce * (goRight ? 1 : -1),
            ForceMode2D.Impulse);
    }

    // public override void ApplyPowerup(MonoBehaviour i)
    // {
    //     PlayerController player = i as PlayerController;
    //     if (player != null) player.MakeSuperMario();
    // }
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