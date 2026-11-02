using UnityEngine;

// plays a one-shot clip on the controller's AudioSource, e.g. as a setup action when transforming
[CreateAssetMenu(menuName = "PluggableSM/Actions/PlaySound")]
public class PlaySoundAction : Action
{
    public AudioClip clip;
    public override void Act(StateController controller)
    {
        controller.GetComponent<AudioSource>().PlayOneShot(clip);
    }
}
