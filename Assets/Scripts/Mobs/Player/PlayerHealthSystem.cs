namespace Mobs.Player
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class PlayerHealthSystem : BaseHealthSystem
    {
        private bool _isConstruct;
        private bool _isLive = true;
        private readonly List<ITakeDamage> _takeDamages = new();
        private readonly List<ITakeHealth> _takeHealths = new();
        private BaseMob _owner;
        [SerializeField] private float _health;
        [SerializeField] private float _baseMinimumHealth;
        [SerializeField] private float _minHealth;
        [SerializeField] private float _baseMaximumHealth;
        [SerializeField] private float _maxHealth;

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
                    Destroy(gameObject);
                }

                if (value >= _maxHealth) value = _maxHealth;

                var oldValue = value;
                _health = value;
                if (_health < oldValue)
                    foreach (var takeDamage in _takeDamages)
                        takeDamage.TakeDamage(this);
                else
                    foreach (var takeHealth in _takeHealths)
                        takeHealth.TakeHealth(this);
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

                var oldValue = value;
                _maxHealth = value;
                if (_maxHealth < oldValue)
                    foreach (var takeDamage in _takeDamages)
                        takeDamage.TakeDamage(this);
                else
                    foreach (var takeHealth in _takeHealths)
                        takeHealth.TakeHealth(this);
            }
        }

        public override BaseHealthSystem Construct(BaseMob owner)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _isConstruct = true;
                _health = _maxHealth;
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
            Health -= damage.TypeDamage switch
            {
                TypesDamage.Physical => damage.CountDamage / 2,
                TypesDamage.Magical => damage.CountDamage * 2,
                TypesDamage.Clear => damage.CountDamage,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Subscribe(ITakeDamage takeDamage)
        {
            _takeDamages.Add(takeDamage);
        }

        public override void Unsubscribe(ITakeDamage takeDamage)
        {
            _takeDamages.Remove(takeDamage);
        }

        public override void Subscribe(ITakeHealth takeHealth)
        {
            _takeHealths.Add(takeHealth);
        }

        public override void Unsubscribe(ITakeHealth takeHealth)
        {
            _takeHealths.Remove(takeHealth);
        }
    }
}