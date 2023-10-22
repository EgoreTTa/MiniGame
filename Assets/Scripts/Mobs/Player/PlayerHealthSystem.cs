namespace Assets.Scripts.Mobs.Player
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using GUI;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class PlayerHealthSystem : BaseHealthSystem
    {
        private bool _isConstruct;
        private bool _isLive = true;
        private ManagerGUI _managerGUI;
        private readonly List<IHealthChangeable> _healthsChangeable = new();
        private BaseMob _owner;
        [SerializeField] private float _health;
        [SerializeField] private float _minHealth;
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

                if (value >= _maxHealth)
                {
                    value = _maxHealth;
                }

                if (value < _health)
                {
                    foreach (var healthChangeable in _healthsChangeable)
                    {
                        healthChangeable.TakeDamage(this);
                    }
                }
                else
                {
                    foreach (var healthChangeable in _healthsChangeable)
                    {
                        healthChangeable.TakeHealth(this);
                    }
                }

                _health = value;
                UpdateSubscribers();
                _managerGUI.UpdateHealthBar(_health, _maxHealth);
            }
        }

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

        public override float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value <= _minHealth) value = _minHealth;
                _maxHealth = value;
                UpdateSubscribers();
                _managerGUI.UpdateHealthBar(_health, _maxHealth);
            }
        }

        public override BaseHealthSystem Construct(BaseMob owner, ManagerGUI managerGUI)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _managerGUI = managerGUI;
                _managerGUI.UpdateHealthBar(_health, _maxHealth);
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
            Health -= damage.TypeDamage switch
            {
                TypesDamage.Physical => damage.CountDamage / 2,
                TypesDamage.Magical => damage.CountDamage * 2,
                TypesDamage.Clear => damage.CountDamage,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public override void Subscribe(IHealthChangeable healthChangeable)
        {
            _healthsChangeable.Add(healthChangeable);
        }

        public override void Unsubscribe(IHealthChangeable healthChangeable)
        {
            _healthsChangeable.Remove(healthChangeable);
        }

        public void UpdateSubscribers()
        {
            foreach (var healthChangeable in _healthsChangeable)
            {
                healthChangeable.UpdateHealthSystem(this);
            }
        }
    }
}