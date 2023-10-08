namespace Assets.Scripts.Items
{
    using Interfaces;
    using JetBrains.Annotations;
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
            owner.GetComponentInChildren<IAttackSystem>().DamageCount += _damageBoost;
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _owner?.Inventory.Take(this);
        }
    }
}