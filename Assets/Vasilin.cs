using UnityEngine;

[DisallowMultipleComponent]
public class Vasilin : BaseItem
{
    public override void PickUp(BaseMob parent)
    {
        parent.Scorer.PickItem(this);
        parent.MaxMoveSpeed++;
        parent.MoveSpeed++;
        parent.TurningSpeed += 15;
    }

    public override void PickDown(BaseMob parent)
    {
        parent.MaxMoveSpeed--;
        parent.MoveSpeed--;
        parent.TurningSpeed -= 15;
    }
}