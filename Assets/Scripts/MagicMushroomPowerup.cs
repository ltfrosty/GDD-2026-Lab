using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    public LayerMask flipOnLayers;
    public Transform spriteChild;
    public float moveForce = 3f;

    private Vector3 startLocalPosition;
    private Vector3 hiddenLocalPosition;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.MagicMushroom;

        startLocalPosition = transform.localPosition;
        if (spriteChild != null)
            hiddenLocalPosition = spriteChild.localPosition;

        Debug.Log($"[{name}] Start captured: startLocal={startLocalPosition}, hidden={hiddenLocalPosition}");

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
    }

    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(Vector2.right * moveForce * (goRight ? 1 : -1), ForceMode2D.Impulse);
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: Super Mario transformation
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

        Debug.Log($"[{name}] ResetPowerup -> localPos={transform.localPosition}, spriteLocal={(spriteChild != null ? spriteChild.localPosition.ToString() : "none")}");
    }
}