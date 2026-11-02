using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Actions/StartStarman")]
public class StartStarmanAction : Action
{
    public AudioClip starmanStart;
    // MusicManager listens to this to swap the theme for the Starman music
    public SimpleGameEvent onStarmanStart;

    public override void Act(StateController controller)
    {
        BuffStateController b = (BuffStateController)controller;
        b.StartStarman(starmanStart);
        if (onStarmanStart != null) onStarmanStart.Raise();
    }
}
