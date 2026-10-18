using UnityEngine;

public class StarmanPowerup : BasePowerup
{
    public GameConstants gameConstants;
    public LayerMask flipOnLayers;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.StarMan;

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

        // vertical surface -> reverse direction
        if ((flipOnLayers.value & (1 << col.gameObject.layer)) != 0
            && Mathf.Abs(contact.normal.x) > 0.5f)
        {
            goRight = !goRight;
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
            rigidBody.AddForce(
                Vector2.right * gameConstants.powerupMoveForce * (goRight ? 1 : -1),
                ForceMode2D.Impulse);
        }
        // landed on something -> hop again
        else if (contact.normal.y > 0.5f)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0f);
            rigidBody.AddForce(
                Vector2.up * gameConstants.starmanBounceForce,
                ForceMode2D.Impulse);
        }
    }

    // Animation Event at the END of the emerge clip
    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;

        // sideways AND upward, so it leaves the box already rising
        rigidBody.AddForce(
            new Vector2(gameConstants.powerupMoveForce * (goRight ? 1 : -1),
                        gameConstants.starmanBounceForce),
            ForceMode2D.Impulse);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        PlayerController player = i as PlayerController;
        if (player != null)
            player.MakeInvincible();
        else
            Debug.LogWarning($"[{name}] ApplyPowerup got {i?.GetType().Name ?? "null"}, expected PlayerController");
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }
}