namespace Items
{
    using Interfaces;
    using JetBrains.Annotations;
    using Mobs;
    using Mobs.Player;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class BerserkerHelm : BaseItem, ITakeDamage, ITakeHealth
    {
        private bool _isActive;
        [SerializeField] private float _damagePercent;
        [SerializeField] [Range(0, 1)] private float _healthThreshold;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            owner.HealthSystem.Subscribe(this as ITakeHealth);
            owner.HealthSystem.Subscribe(this as ITakeDamage);
            CheckActivationOptions(owner.HealthSystem);
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            owner.HealthSystem.Unsubscribe(this as ITakeHealth);
            owner.HealthSystem.Unsubscribe(this as ITakeDamage);
            owner.Inventory.Take(this);
        }

        private void Activate()
        {
            _owner.Attribute.AttackRelativeDamage += _damagePercent;
            _isActive = true;
        }

        private void Deactivate()
        {
            _owner.Attribute.AttackRelativeDamage -= _damagePercent;
            _isActive = false;
        }

        private void CheckActivationOptions(BaseHealthSystem healthSystem)
        {
            if (healthSystem.Health / healthSystem.MaxHealth <= _healthThreshold)
            {
                if (_isActive is false) Activate();
            }
            else
            {
                if (_isActive is true) Deactivate();
            }
        }

        public void TakeHealth(BaseHealthSystem healthSystem)
        {
            CheckActivationOptions(healthSystem);
        }

        public void TakeDamage(BaseHealthSystem healthSystem)
        {
            CheckActivationOptions(healthSystem);
        }
    }
}