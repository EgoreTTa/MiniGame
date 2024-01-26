namespace Mobs.Enemies
{
    using System;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultEnemyHealthSystem : BaseHealthSystem
    {
        private bool _isConstruct;
        private BaseMob _owner;
        [SerializeField] private float _health;
        [SerializeField] private float _baseMinimumHealth;
        [SerializeField] private float _minHealth;
        [SerializeField] private float _baseMaximumHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private bool _isLive;
        [SerializeField] private BaseMob _lastDamageDealt;

        public override bool IsLive => _isLive;

        public override float Health
        {
            get => _health;
            protected set
            {
                if (value <= _minHealth)
                {
                    value = _minHealth;
                    _isLive = false;
                    Dead();
                }

                if (value >= _maxHealth) value = _maxHealth;

                _health = value;
            }
        }

        public override float BaseMinHealth => _baseMinimumHealth;

        public override float MinHealth
        {
            get => _minHealth;
            set
            {
                if (value <= 0) value = 0;
                if (value > _maxHealth) value = _maxHealth;
                _minHealth = value;
            }
        }

        public override float BaseMaxHealth => _baseMaximumHealth;

        public override float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value <= _minHealth) value = _minHealth;
                _maxHealth = value;
            }
        }

        public override BaseHealthSystem Construct(BaseMob owner)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public override void TakeHealth(Health health)
        {
            Health += health.CountHealth;
        }

        public override void TakeDamage(Damage damage)
        {
            _lastDamageDealt = damage.Owner;
            Health -= damage.TypeDamage switch
            {
                TypesDamage.Physical => damage.CountDamage / 2,
                TypesDamage.Magical => damage.CountDamage * 2,
                TypesDamage.Clear => damage.CountDamage,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Subscribe(ITakeDamage takeDamage) { }

        public override void Unsubscribe(ITakeDamage takeDamage) { }

        public override void Subscribe(ITakeHealth takeHealth) { }

        public override void Unsubscribe(ITakeHealth takeHealth) { }

        private void Dead()
        {
            Destroy(gameObject);
            _lastDamageDealt?.KilledMob(_owner);
        }
    }
}