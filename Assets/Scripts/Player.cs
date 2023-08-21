using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : BaseMob
{
    [SerializeField] private StatesOfPlayer _stateOfPlayer;
    private IDash _dash;
    private IAttack _attack;
    private IAbility _ability1;
    private bool _isInteract;

    public enum StatesOfPlayer
    {
        Idle,
        Move,
        Dash,
        Attack,
        Interaction,
    }

    public StatesOfPlayer StateOfPlayer => _stateOfPlayer;

    private void Awake()
    {
        _inventory = new Inventory(this);
        if (GetComponent<IDash>() is { } iDash) _dash = iDash;
        else throw new Exception("Player not instance IDash");
        if (GetComponent<IAttack>() is { } iAttack) _attack = iAttack;
        else throw new Exception("Player not instance IAttack");
        _ability1 = GetComponent<IAbility>();
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
        var isDash = Input.GetKeyDown(KeyCode.Space);

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
                
                if (isDash)
                {
                    _dash.Dash();
                    _stateOfPlayer = StatesOfPlayer.Dash;
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
            case StatesOfPlayer.Dash:
                if (_dash.StateOfDash == StatesOfDash.Idle)
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

    protected override void DecreaseHealth()
    {
        Scorer.ClearStreak();
    }
}