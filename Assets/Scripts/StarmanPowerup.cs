using UnityEngine;

public class StarmanPowerup : BasePowerup
{
    public LayerMask flipOnLayers;
    public Transform spriteChild;
    public float moveForce = 3f;
    public float bounceForce = 8f;

    private Vector3 startLocalPosition;
    private Vector3 hiddenLocalPosition;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.StarMan;

        startLocalPosition = transform.localPosition;
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

        if ((flipOnLayers.value & (1 << col.gameObject.layer)) != 0
            && Mathf.Abs(contact.normal.x) > 0.5f)
        {
            goRight = !goRight;
            rigidBody.linearVelocity = new Vector2(0, rigidBody.linearVelocity.y);
            rigidBody.AddForce(Vector2.right * moveForce * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
        else if (contact.normal.y > 0.5f)
        {
            rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, 0f);
            rigidBody.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }

    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(
            new Vector2(moveForce * (goRight ? 1 : -1), bounceForce),
            ForceMode2D.Impulse);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: invincibility
    }

    public override void ResetPowerup()
    {
        base.ResetPowerup();

        gameObject.SetActive(true);

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;

        transform.localPosition = startLocalPosition;
        if (spriteChild != null)
            spriteChild.localPosition = hiddenLocalPosition;

    }
}