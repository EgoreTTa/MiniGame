namespace Assets.Scripts
{
    using NoMonoBehaviour;
    using System;
    using Enemies;
    using Enums;
    using Interfaces;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class Player : BaseMob
    {
        [SerializeField] private StatesOfPlayer _stateOfPlayer;
        private IJerk _jerk;
        private IAttack _attack;
        private IAbility _ability1;
        private bool _isInteract;

        public enum StatesOfPlayer
        {
            Idle,
            Move,
            Jerk,
            Attack,
            Interaction,
        }

        public StatesOfPlayer StateOfPlayer => _stateOfPlayer;

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
                        Walk(axis);
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

        protected override void DecreaseHealth() { }
    }
}