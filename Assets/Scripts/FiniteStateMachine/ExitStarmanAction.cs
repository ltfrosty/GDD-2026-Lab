using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ExitStarman")]
public class ExitStarmanAction : Action
{
    public AudioClip starmanEnd;
    public override void Act(StateController controller)
    {
        BuffStateController b = (BuffStateController)controller;
        b.ExitStarman(starmanEnd);
    }
}
