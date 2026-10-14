using UnityEngine;

public class StarmanPowerup : BasePowerup
{
    public LayerMask flipOnLayers;
    public Transform spriteChild;
    public float moveForce = 3f;
    public float bounceForce = 5f;

    private Vector3 hiddenLocalPosition;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.StarMan;

        if (spriteChild != null)
            hiddenLocalPosition = spriteChild.localPosition;

        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned) return;

        if (col.gameObject.CompareTag("Player"))
        {
            ApplyPowerup(col.gameObject.GetComponent<PlayerMovement>());
            gameObject.SetActive(false);
            return;
        }

        ContactPoint2D contact = col.GetContact(0);

        // vertical surface -> reverse direction
        if ((flipOnLayers.value & (1 << col.gameObject.layer)) != 0
            && Mathf.Abs(contact.normal.x) > 0.5f)
        {
            goRight = !goRight;
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
            rigidBody.AddForce(Vector2.right * moveForce * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
        // landed on something -> hop again (the starman's signature bounce)
        else if (contact.normal.y > 0.5f)
        {
            rigidBody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }

    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(Vector2.right * moveForce, ForceMode2D.Impulse);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: invincibility — not implemented yet
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();

        gameObject.SetActive(true);

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;

        transform.localPosition = Vector3.zero;
        if (spriteChild != null)
            spriteChild.localPosition = hiddenLocalPosition;
    }
}