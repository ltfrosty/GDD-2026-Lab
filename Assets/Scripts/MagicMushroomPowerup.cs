using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    public LayerMask flipOnLayers;        // pipes etc. — set in Inspector
    public Transform spriteChild;         // the Mushroom child
    public float moveForce = 3f;

    private Vector3 hiddenLocalPosition;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;

        if (spriteChild != null)
            hiddenLocalPosition = spriteChild.localPosition;

        // inert until the emerge animation finishes
        rigidBody.simulated = false;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!spawned) return;

        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            ApplyPowerup(col.gameObject.GetComponent<PlayerMovement>());
            gameObject.SetActive(false);   // hide, not destroy, so restart can restore it

        }
        else if ((flipOnLayers.value & (1 << col.gameObject.layer)) != 0)
        {
            goRight = !goRight;
            rigidBody.linearVelocity = Vector2.zero;
            rigidBody.AddForce(Vector2.right * moveForce * (goRight ? 1 : -1), ForceMode2D.Impulse);
        }
    }

    // interface implementation
    // called by an Animation Event at the END of the emerge clip
    public override void SpawnPowerup()
    {
        spawned = true;

        rigidBody.simulated = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(Vector2.right * moveForce, ForceMode2D.Impulse);
    }

    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

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