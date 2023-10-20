namespace Assets.Scripts.Mobs.Player
{
    using NoMonoBehaviour;
    using System;
    using Abilities;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using Items;
    using Jerks;
    using Movements;
    using UnityEngine.SceneManagement;
    using Attacks;
    using GUI;

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [DisallowMultipleComponent]
    public class Player : BaseMob
    {
        public enum StatesOfPlayer
        {
            Idle,
            Move,
            Jerk,
            Attack,
            Interaction,
        }

        private bool _isInteract;
        private IInteraction _interaction;
        private Inventory _inventory;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private StatesOfPlayer _stateOfPlayer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _trigger;
        [SerializeField] private BaseHealthSystem _healthSystem;
        [SerializeField] private BaseMovement _movementSystem;
        [SerializeField] private BaseJerk _jerk;
        [SerializeField] private BaseAttackSystem _attackSystem;
        [SerializeField] private BaseAbility _ability1;
        [SerializeField] private ManagerGUI _managerGUI;

        public override string FirstName => _firstname;
        public override GroupsMobs GroupMobs => _groupMobs;
        public StatesOfPlayer StateOfPlayer => _stateOfPlayer;
        public override BaseHealthSystem HealthSystem => _healthSystem;
        public override BaseMovement MovementSystem => _movementSystem;
        public override BaseAttackSystem AttackSystem => _attackSystem;
        public Inventory Inventory => _inventory;

        private void Awake()
        {
            _inventory = new Inventory(this);
            _healthSystem.Construct(_managerGUI);
            _movementSystem.Construct(transform, _rigidbody);
            _jerk.Construct(this, transform, _rigidbody, _movementSystem);
            _attackSystem.Construct(this, _groupMobs, _healthSystem, transform);
            _ability1.Construct(this, gameObject);
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
                        _jerk.Jerk(transform.up);
                        _stateOfPlayer = StatesOfPlayer.Jerk;
                        return;
                    }

                    if (axis != Vector3.zero)
                    {
                        _movementSystem.Move(axis);
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
                    if (isKeyInteraction)
                    {
                        _stateOfPlayer = StatesOfPlayer.Idle;
                        _interaction?.Interact(this);
                        _isInteract = false; 
                        return;
                    }

                    if (_isInteract is false)
                    {
                        _stateOfPlayer = StatesOfPlayer.Idle;
                        return;
                    }

                    Interaction();
                    break;
                default:
                    throw new Exception($"{nameof(StatesOfPlayer)} of {nameof(Player)}: not valid state");
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
                        Debug.Log("Trade");
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