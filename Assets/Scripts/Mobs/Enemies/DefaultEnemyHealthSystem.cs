namespace Assets.Scripts.Mobs.Enemies
{
    using System;
    using GUI;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultEnemyHealthSystem : BaseHealthSystem
    {
        private bool _isConstruct;
        [SerializeField] private float _health;
        [SerializeField] private float _minHealth;
        [SerializeField] private float _maxHealth;
        [SerializeField] private bool _isLive;

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

                _health = value;
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
            }
        }

        public override BaseHealthSystem Construct(ManagerGUI managerGUI = null)
        {
            if (_isConstruct is false)
            {
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
    }
}