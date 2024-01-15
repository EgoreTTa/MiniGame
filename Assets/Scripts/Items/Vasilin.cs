namespace Items
{
    using JetBrains.Annotations;
    using Mobs.Player;
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
            owner.MovementSystem.MaxMoveSpeed += _changeMaxMoveSpeed;
            owner.MovementSystem.MoveSpeed += _changeMoveSpeed;
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            owner.MovementSystem.MaxMoveSpeed -= _changeMaxMoveSpeed;
            owner.MovementSystem.MoveSpeed -= _changeMoveSpeed;
        }
    }
}