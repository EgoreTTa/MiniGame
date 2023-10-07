namespace Assets.Scripts
{
    using NoMonoBehaviour;
    using System;
    using Enums;
    using Interfaces;
    using UnityEngine;
    using Items;

    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IMob
    {
        public enum StatesOfPlayer
        {
            Idle,
            Move,
            Jerk,
            Attack,
            Interaction,
        }

        private string _firstname;
        private IHealthSystem _healthSystem;
        private IMoveSystem _moveSystem;
        private IJerk _jerk;
        private IAttackSystem _attackSystem;
        private IAbility _ability1;
        private bool _isInteract;
        private IInteraction _interaction;
        private Inventory _inventory;
        [SerializeField] private StatesOfPlayer _stateOfPlayer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private Collider2D _trigger;

        public string FirstName { get; }
        public GroupsMobs GroupMobs { get; }
        public StatesOfPlayer StateOfPlayer => _stateOfPlayer;
        public IMoveSystem MoveSystem => _moveSystem;
        public Inventory Inventory => _inventory;

        private void Awake()
        {
            _inventory = new Inventory(this);
            if (GetComponent<IJerk>() is { } jerk) _jerk = jerk;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IJerk)}");
            if (GetComponent<IMoveSystem>() is { } moveSystem) _moveSystem = moveSystem;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IMoveSystem)}");
            if (GetComponent<IHealthSystem>() is { } healthSystem) _healthSystem = healthSystem;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IHealthSystem)}");
            if (GetComponentInChildren<IAttackSystem>() is { } attackSystem) _attackSystem = attackSystem;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IAttackSystem)}");
            if (GetComponentInChildren<IAbility>() is { } ability1) _ability1 = ability1;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IAbility)}");
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
                    throw new Exception($"{nameof(StateOfPlayer)} of {nameof(Player)}: not valid state");
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
    }
}