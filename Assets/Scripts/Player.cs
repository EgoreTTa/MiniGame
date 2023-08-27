namespace Assets.Scripts
{
    using NoMonoBehaviour;
    using System;
    using Enemies;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using Items;
    using GUI;

    [DisallowMultipleComponent]
    public class Player : BaseMob, IHealthSystem, IMoveSystem
    {
        [SerializeField] private StatesOfPlayer _stateOfPlayer;
        private IJerk _jerk;
        private IAttack _attack;
        private IAbility _ability1;
        private bool _isInteract;
        private IInteraction _interaction;
        private Inventory _inventory;
        [SerializeField] private ManagerGUI _managerGUI;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _trigger;

        public enum StatesOfPlayer
        {
            Idle,
            Move,
            Jerk,
            Attack,
            Interaction,
        }

        public StatesOfPlayer StateOfPlayer => _stateOfPlayer;

        public float Health
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

        private void Awake()
        {
            _inventory = new Inventory(this);
            if (GetComponent<IJerk>() is { } iJerk) _jerk = iJerk;
            else throw new Exception("Player not instance IJerk");
            if (GetComponentInChildren<IAttack>() is { } iAttack) _attack = iAttack;
            else throw new Exception("Player not instance IAttack");
            if (GetComponentInChildren<IAbility>() is { } iAbility) _ability1 = iAbility;
            else throw new Exception("Player not instance IAbility");
        }

        private void Update()
        {
            ActionChoice();
        }

        private void ActionChoice()
        {
            var axis = Vector3.zero;
            if (Input.GetKey(KeyCode.D)) axis.x++;
            if (Input.GetKey(KeyCode.A)) axis.x--;
            if (Input.GetKey(KeyCode.W)) axis.y++;
            if (Input.GetKey(KeyCode.S)) axis.y--;

            var isAttack = Input.GetKeyDown(KeyCode.E);
            var isJerk = Input.GetKeyDown(KeyCode.Space);

            var isInteraction = Input.GetKeyDown(KeyCode.I);

            var isAbility1 = Input.GetKeyDown(KeyCode.Alpha1);

            switch (_stateOfPlayer)
            {
                case StatesOfPlayer.Idle or StatesOfPlayer.Move:
                    if (isAttack)
                    {
                        _attack.Attack();
                        _stateOfPlayer = StatesOfPlayer.Attack;
                        return;
                    }

                    if (isAbility1)
                    {
                        _ability1?.Cast();
                        _stateOfPlayer = StatesOfPlayer.Attack;
                        return;
                    }

                    if (isJerk)
                    {
                        _jerk.Jerk();
                        _stateOfPlayer = StatesOfPlayer.Jerk;
                        return;
                    }

                    if (axis != Vector3.zero)
                    {
                        Move(axis);
                        _stateOfPlayer = StatesOfPlayer.Move;
                        return;
                    }

                    if (isInteraction)
                    {
                        Interaction();
                        _stateOfPlayer = StatesOfPlayer.Interaction;
                        return;
                    }

                    _stateOfPlayer = StatesOfPlayer.Idle;
                    break;
                case StatesOfPlayer.Jerk:
                    if (_jerk.StateOfJerk == StatesOfJerk.Idle)
                    {
                        _stateOfPlayer = StatesOfPlayer.Idle;
                    }

                    break;
                case StatesOfPlayer.Attack:
                    if (_attack.StateOfAttack == StatesOfAttack.Idle
                        &&
                        _ability1.StateOfAbility == StatesOfAbility.Standby)
                    {
                        _stateOfPlayer = StatesOfPlayer.Idle;
                    }

                    break;
                case StatesOfPlayer.Interaction:
                    if (_isInteract is false)
                    {
                        _stateOfPlayer = StatesOfPlayer.Idle;
                        return;
                    }

                    Interaction();
                    break;
                default:
                    throw new Exception("FSM of Player: not valid state");
            }
        }

        private void Interaction()
        {
            if (_interaction != null)
            {
                if (_interaction.IsInteract is false
                    ||
                    _isInteract is false)
                {
                    if (_isInteract is false)
                    {
                        Debug.Log($"{_firstname} обратился к {_interaction.FirstName}");
                        _interaction.Interact(this);
                        _isInteract = true;
                    }
                    else
                    {
                        _isInteract = false;
                    }
                }
            }
            else
            {
                _isInteract = false;
            }
        }

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

        public void Move(Vector3 vector)
        {
            _direction = vector.normalized;
            transform.up = _direction;
            transform.position += _direction * _moveSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_trigger.IsTouching(collider))
                if (collider.gameObject.GetComponent<IInteraction>() is { } interaction)
                {
                    _interaction = interaction;
                }

            if (_collider.IsTouching(collider))
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