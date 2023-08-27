namespace Assets.Scripts.Items
{
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Vasilin : BaseItem
    {
        [SerializeField] private float _changeMaxMoveSpeed;
        [SerializeField] private float _changeMoveSpeed;

        public override void PickUp(Player parent)
        {
            parent.MaxMoveSpeed += _changeMaxMoveSpeed;
            parent.MoveSpeed += _changeMoveSpeed;
        }

        public override void PickDown(Player parent)
        {
            parent.MaxMoveSpeed -= _changeMaxMoveSpeed;
            parent.MoveSpeed -= _changeMoveSpeed;
        }
    }
}