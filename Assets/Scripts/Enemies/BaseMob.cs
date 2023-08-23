namespace Assets.Scripts.Enemies
{
    using System;
    using Enums;
    using GUI;
    using Interfaces;
    using Items;
    using NoMonoBehaviour;
    using UnityEngine;

    public abstract class BaseMob : MonoBehaviour, IHealthSystem
    {
        [SerializeField] protected float _health;
        [SerializeField] protected float _minHealth;
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected int _stamina;
        [SerializeField] protected int _minStamina;
        [SerializeField] protected int _maxStamina;
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected float _minMoveSpeed;
        [SerializeField] protected float _maxMoveSpeed;
        [SerializeField] protected string _firstname;
        [SerializeField] protected float _damageCount;
        [SerializeField] protected float _viewRadius;
        [SerializeField] protected GroupsMobs _groupMobs;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _trigger;
        protected bool _live = true;
        protected IInteraction _interaction;
        private Vector3 _direction = Vector3.up;
        protected Inventory _inventory;
        [SerializeField] protected ManagerGUI _managerGUI;

        public Vector3 Direction => _direction;

        public float Health
        {
            get => _health;
            protected set
            {
                if (value < _health) DecreaseHealth();
                if (value <= _minHealth)
                {
                    value = _minHealth;
                    _live = false;
                    Destroy(gameObject);
                }

                if (value >= _maxHealth)
                {
                    value = _maxHealth;
                }
                
                _health = value;
                _managerGUI?.UpdateHealthBar(_health, _maxHealth);
            }
        }

        public float MinHealth
        {
            get => _minHealth;
            set
            {
                if (value <= 0) value = 0;
                if (value > _maxHealth) value = _maxHealth;
                _minHealth = value;
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value <= _minHealth) value = _minHealth;
                _maxHealth = value;
                _managerGUI?.UpdateHealthBar(_health, _maxHealth);
            }
        }

        public int Stamina
        {
            get => _stamina;
            set
            {
                if (value < _minStamina) value = _minStamina;
                if (value > _maxStamina) value = _maxStamina;
                _stamina = value;
            }
        }

        public int MinStamina
        {
            get => _minStamina;
            set
            {
                if (value <= 0) value = 1;
                if (value > _maxStamina) value = _maxStamina;
                _stamina = value;
            }
        }

        public int MaxStamina
        {
            get => _maxStamina;
            set
            {
                if (value <= _minStamina) value = _minStamina += 1;
                _maxStamina = value;
            }
        }

        public float MoveSpeed
        {
            get => _moveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _moveSpeed = value;
            }
        }

        public float MinMoveSpeed
        {
            get => _minMoveSpeed;
            set
            {
                if (value < 0) value = 0;
                if (value > _maxMoveSpeed) value = _maxMoveSpeed;
                _minMoveSpeed = value;
            }
        }

        public float MaxMoveSpeed
        {
            get => _maxMoveSpeed;
            set
            {
                if (value < _minMoveSpeed) value = _minMoveSpeed;
                _maxMoveSpeed = value;
            }
        }

        public string Firstname => _firstname;

        public float DamageCount
        {
            get => _damageCount;
            set
            {
                if (value < 0) value = 0;
                _damageCount = value;
            }
        }

        public float ViewRadius
        {
            get => _viewRadius;
            set
            {
                if (value < 0) value = 0;
                _viewRadius = value;
            }
        }

        public bool Live => _live;
        public GroupsMobs GroupMobs => _groupMobs;

        protected virtual void DecreaseHealth() { }

        public void TakeHealth(Health health)
        {
            Health += health.CountHealth;
        }

        public void TakeDamage(Damage damage)
        {
            Health -= damage.TypeDamage switch
            {
                TypesDamage.Physical => damage.CountDamage / 2,
                TypesDamage.Magical => damage.CountDamage * 2,
                TypesDamage.Clear => damage.CountDamage,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected virtual void Walk(Vector3 vector)
        {
            _direction = vector.normalized;
            transform.up = _direction;
            transform.position += _direction * _moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_trigger != null 
                &&
                _trigger.IsTouching(collider))
                if (collider.gameObject.GetComponent<IInteraction>() is { } interaction)
                {
                    _interaction = interaction;
                }

            if (_collider != null
                &&
                _collider.IsTouching(collider))
                if (collider.gameObject.GetComponent<BaseItem>() is { } item)
                {
                    _inventory?.Put(item);
                    if (item.GetComponent<IUsable>() is { } usable)
                    {
                        usable.Use(this);
                    }

                    if (item.GetComponent<IEquipment>() is { } equipment)
                    {
                        _inventory?.Equip(equipment);
                    }
                }
        }
    }
}