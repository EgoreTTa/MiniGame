using UnityEngine;

[DisallowMultipleComponent]
public class Vasilin : BaseItem
{
    [SerializeField] private float _changeMaxMoveSpeed;
    [SerializeField] private float _changeMoveSpeed;
    [SerializeField] private float _changeTurningSpeed;

    public override void PickUp(BaseMob parent)
    {
        parent.Scorer.PickItem(this);
        parent.MaxMoveSpeed += _changeMaxMoveSpeed;
        parent.MoveSpeed += _changeMoveSpeed;
        parent.TurningSpeed += _changeTurningSpeed;
    }

    public override void PickDown(BaseMob parent)
    {
        parent.MaxMoveSpeed -= _changeMaxMoveSpeed;
        parent.MoveSpeed -= _changeMoveSpeed;
        parent.TurningSpeed -= _changeTurningSpeed;
    }
}