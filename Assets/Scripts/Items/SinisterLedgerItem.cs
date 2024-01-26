namespace Items
{
    using Interfaces;
    using JetBrains.Annotations;
    using Mobs;
    using Mobs.Player;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class SinisterLedger : BaseItem, IKillerMob, ITakeDamage
    {
        [SerializeField] private float _powerDamagePercent;
        [SerializeField] private float _incrementDamagePercent;
        [SerializeField] private float _maxDamagePercent;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            owner.Subscribe(this);
            owner.HealthSystem.Subscribe(this);
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            owner.Unsubscribe(this);
            owner.HealthSystem.Unsubscribe(this);
            owner.Inventory.Take(this);
            ResetProgress();
        }

        public void Killer(BaseMob whomKilled)
        {
            if (_powerDamagePercent < _maxDamagePercent)
            {
                _owner.Attribute.AttackRelativeDamage += _incrementDamagePercent;
                _powerDamagePercent += _incrementDamagePercent;
            }
        }

        public void TakeDamage(BaseHealthSystem healthSystem) => ResetProgress();

        public void ResetProgress()
        {
            _owner.Attribute.AttackRelativeDamage -= _powerDamagePercent;
            _powerDamagePercent = default;
        }
    }
}