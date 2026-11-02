using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/ExitStarman")]
public class ExitStarmanAction : Action
{
    public AudioClip starmanEnd;
    // MusicManager listens to this to go back to the theme music
    public SimpleGameEvent onStarmanEnd;

    public override void Act(StateController controller)
    {
        BuffStateController b = (BuffStateController)controller;
        b.ExitStarman(starmanEnd);
        if (onStarmanEnd != null) onStarmanEnd.Raise();
    }
}
