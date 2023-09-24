namespace Assets.Scripts.Items
{
    using JetBrains.Annotations;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class Vasilin : BaseItem
    {
        [SerializeField] private float _changeMaxMoveSpeed;
        [SerializeField] private float _changeMoveSpeed;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            owner.MoveSystem.MaxMoveSpeed += _changeMaxMoveSpeed;
            owner.MoveSystem.MoveSpeed += _changeMoveSpeed;
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            owner.MoveSystem.MaxMoveSpeed -= _changeMaxMoveSpeed;
            owner.MoveSystem.MoveSpeed -= _changeMoveSpeed;
        }
    }
}