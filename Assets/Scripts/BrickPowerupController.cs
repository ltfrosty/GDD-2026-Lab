using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class BrickPowerupController : BasePowerupController
{
    [Header("Breaking")]
    public bool breakable = true;
    public GameObject debrisPrefab;

    // an empty brick can be bumped forever; one with a powerup is spent after use
    protected override bool CanBeHit() => !HasPowerup || !used;

    // called when Super Mario hits an empty brick
    public void Break()
    {
        if (!breakable || HasPowerup) return;

        if (debrisPrefab != null)
            Instantiate(debrisPrefab, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }
}