namespace Assets.Scripts.Items
{
    using Enemies;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Vasilin : BaseItem
    {
        [SerializeField] private float _changeMaxMoveSpeed;
        [SerializeField] private float _changeMoveSpeed;

        public override void PickUp(BaseMob parent)
        {
            parent.MaxMoveSpeed += _changeMaxMoveSpeed;
            parent.MoveSpeed += _changeMoveSpeed;
        }

        public override void PickDown(BaseMob parent)
        {
            parent.MaxMoveSpeed -= _changeMaxMoveSpeed;
            parent.MoveSpeed -= _changeMoveSpeed;
        }
    }
}