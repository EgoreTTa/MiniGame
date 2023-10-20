namespace Assets.Scripts.Items
{
    using Interfaces;
    using JetBrains.Annotations;
    using Mobs.Player;
    using UnityEngine;

    [UsedImplicitly]
    [DisallowMultipleComponent]
    public class BerserkerHelm : BaseItem, IHealthChangeable
    {
        private bool _isActive;
        [SerializeField] private float _damagePercent;
        [SerializeField] [Range(0, 1)] private float _healthThreshold;

        public override void PickUp(Player owner)
        {
            base.PickUp(owner);
            owner.HealthSystem.Subscribe(this);
            UpdateHealthSystem(owner.HealthSystem);
        }

        public override void PickDown(Player owner)
        {
            base.PickDown(owner);
            owner.HealthSystem.Unsubscribe(this);
        }

        private void Activate()
        {
            _owner.AttackSystem.DamagePercent += _damagePercent;
            _isActive = true;
        }

        private void Deactivate()
        {
            _owner.AttackSystem.DamagePercent -= _damagePercent;
            _isActive = false;
        }

        public void UpdateHealthSystem(BaseHealthSystem healthSystem)
        {
            if (healthSystem.Health / healthSystem.MaxHealth <= _healthThreshold)
            {
                if (_isActive is false)
                    Activate();
            }
            else
            {
                if (_isActive is true)
                    Deactivate();
            }
        }

        private void OnDestroy()
        {
            _owner?.HealthSystem.Unsubscribe(this);
            _owner?.Inventory.Take(this);
        }
    }
}