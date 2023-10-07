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
    using UnityEngine.SceneManagement;

    [DisallowMultipleComponent]
    public class Player : BaseMob, IHealthSystem
    {
        [SerializeField] private StatesOfPlayer _stateOfPlayer;
        private IJerk _jerk;
        private IMoveSystem _moveSystem;
        private IAttackSystem _attackSystem;
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

        public IMoveSystem MoveSystem => _moveSystem;

        public Inventory Inventory => _inventory;

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

        private void Awake()
        {
            _inventory = new Inventory(this);
            if (GetComponent<IJerk>() is { } iJerk) _jerk = iJerk;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IJerk)}");
            if (GetComponent<IMoveSystem>() is { } iMoveSystem) _moveSystem = iMoveSystem;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IMoveSystem)}");
            if (GetComponentInChildren<IAttackSystem>() is { } iAttack) _attackSystem = iAttack;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IAttackSystem)}");
            if (GetComponentInChildren<IAbility>() is { } iAbility) _ability1 = iAbility;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IAbility)}");
        }

        private void Start()
        {
            _managerGUI?.UpdateHealthBar(_health, _maxHealth);
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

            var isKeyAttack = Input.GetKeyDown(KeyCode.E);
            var isKeyJerk = Input.GetKeyDown(KeyCode.Space);
            var isKeyInteraction = Input.GetKeyDown(KeyCode.I);
            var isKeyAbility1 = Input.GetKeyDown(KeyCode.Alpha1);

            switch (_stateOfPlayer)
            {
                case StatesOfPlayer.Idle or StatesOfPlayer.Move:
                    if (isKeyAttack)
                    {
                        _attackSystem.Attack();
                        _stateOfPlayer = StatesOfPlayer.Attack;
                        return;
                    }

                    if (isKeyAbility1)
                    {
                        _ability1?.Cast();
                        _stateOfPlayer = StatesOfPlayer.Attack;
                        return;
                    }

                    if (isKeyJerk)
                    {
                        _jerk.Jerk();
                        _stateOfPlayer = StatesOfPlayer.Jerk;
                        return;
                    }

                    if (axis != Vector3.zero)
                    {
                        _moveSystem.Move(axis);
                        _stateOfPlayer = StatesOfPlayer.Move;
                        return;
                    }

                    if (isKeyInteraction)
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
                    if (isKeyAttack)
                    {
                        _attackSystem.Attack();
                    }

                    if (_attackSystem.StateOfAttack == StatesOfAttack.Idle
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

        private void OnDestroy()
        {
            if (SceneManager.sceneCount == SceneManager.loadedSceneCount)
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}