using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class QuestionBoxPowerupController : BasePowerupController
{
    [Tooltip("Name of the box animator's idle/blinking state")]
    public string idleState = "blink";

    protected override bool CanBeHit() => !used;

    public override void GameRestart()
    {
        base.GameRestart();

        if (boxAnimator != null)
            boxAnimator.Play(idleState, 0, 0f);
    }
}