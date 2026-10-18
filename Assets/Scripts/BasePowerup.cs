using UnityEngine;
using UnityEngine.Events;

public abstract class BasePowerup : MonoBehaviour, IPowerup
{
    public PowerupType type;
    public bool spawned = false;
    protected bool consumed = false;
    protected bool goRight = true;
    protected Rigidbody2D rigidBody;

    [Header("Events raised by this powerup")]
    public UnityEvent<IPowerup> powerupCollected;

    protected Vector3 startLocalPosition;
    protected Vector3 hiddenLocalPosition;

    public Transform spriteChild;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        startLocalPosition = transform.localPosition;
        if (spriteChild != null)
            hiddenLocalPosition = spriteChild.localPosition;
    }

    public PowerupType powerupType => type;
    public bool hasSpawned => spawned;

    public void DestroyPowerup() => Destroy(this.gameObject);

    protected void Collect()
    {
        if (consumed) return;
        consumed = true;

        powerupCollected.Invoke(this);
        gameObject.SetActive(false);
    }

    public virtual void ResetPowerup()
    {
        spawned = false;
        consumed = false;
        goRight = true;

        gameObject.SetActive(true);

        transform.localPosition = startLocalPosition;
        if (spriteChild != null)
            spriteChild.localPosition = hiddenLocalPosition;
    }

    public abstract void SpawnPowerup();
    public abstract void ApplyPowerup(MonoBehaviour i);
}