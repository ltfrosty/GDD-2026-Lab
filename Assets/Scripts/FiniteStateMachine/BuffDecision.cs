using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Buff")]
public class BuffDecision : Decision
{
    public PowerupType buffCollected;

    public override bool Decide(StateController controller)
    {
        BuffStateController b = (BuffStateController)controller;
        return b.currentBuffType == buffCollected;
    }
}
