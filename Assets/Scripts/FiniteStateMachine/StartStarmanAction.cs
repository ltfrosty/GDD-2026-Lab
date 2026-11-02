using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStarman")]
public class StartStarmanAction : Action
{
    public AudioClip starmanStart;
    public override void Act(StateController controller)
    {
        BuffStateController b = (BuffStateController)controller;
        b.StartStarman(starmanStart);
    }
}
