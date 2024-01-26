namespace Items
{
    using JetBrains.Annotations;
    using Mobs.Player;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class MonsterEye : BaseItem

    {
        [SerializeField] private float _abilityRadius;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            _owner = owner;
            _owner.Attribute.AbilityRadius += _abilityRadius;
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            _owner = owner;
            _owner.Attribute.AbilityRadius -= _abilityRadius;
        }
    }
}