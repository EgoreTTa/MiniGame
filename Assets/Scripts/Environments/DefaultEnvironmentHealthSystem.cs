namespace Assets.Scripts.Environments
{
    using System;
    using GUI;
    using Enums;
    using Interfaces;
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultEnvironmentHealthSystem : BaseHealthSystem
    {
        private bool _isConstruct;
        private bool _isLive;
        [SerializeField] private float _health;
        [SerializeField] private float _minHealth;
        [SerializeField] private float _maxHealth;

        public override bool IsLive => _isLive;

        public override float Health
        {
            get => _health;
            protected set
            {
                if (value < _minHealth)
                {
                    value = _minHealth;
                    Destruction();
                }

                if (value > _maxHealth)
                {
                    value = _maxHealth;
                }

                _health = value;
            }
        }

        public override float MinHealth
        {
            get => _minHealth;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxHealth) value = _maxHealth;
                _minHealth = value;
            }
        }

        public override float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value < _maxHealth) value = _maxHealth;
                _maxHealth = value;
            }
        }

        public override BaseHealthSystem Construct(BaseMob owner = null, ManagerGUI managerGUI = null)
        {
            if (_isConstruct is false)
            {
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public override void TakeHealth(Health health) { }

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

        public override void Subscribe(IHealthChangeable healthChangeable) { }

        public override void Unsubscribe(IHealthChangeable healthChangeable) { }

        private void Destruction()
        {
            GetComponent<SpriteRenderer>().color = Color.white.linear * .2f;
            ItemDropSystem.Drop(transform.position);
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());
            Destroy(this);
        }
    }
}
