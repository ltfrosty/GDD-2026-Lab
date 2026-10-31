using UnityEngine;

[CreateAssetMenu(
    fileName = "BoolVariable",
    menuName = "ScriptableObjects/BoolVariable",
    order = 2)]
public class BoolVariable : Variable<bool>
{
    public override void SetValue(bool value)
    {
        _value = value;
    }
}