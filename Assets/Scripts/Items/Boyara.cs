namespace Items
{
    using Attacks;
    using JetBrains.Annotations;
    using Mobs.Player;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class Boyara : BaseItem
    {
        private int _damageBoost;
        [SerializeField] private int _minRange;
        [SerializeField] private int _maxRange;

        private void Start()
        {
            _damageBoost = Random.Range(_minRange, _maxRange);
        }

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            owner.GetComponentInChildren<BaseAttackSystem>().DamageCount += _damageBoost;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _owner?.Inventory.Take(this);
        }
    }
}