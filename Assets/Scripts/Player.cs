using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Player : BaseMob
{
    [SerializeField] private float _increaseCharacteristicsOnMurder;
    [SerializeField] private float _intervalForAttack;
    private float _timerIntervalForAttack;
    private Inventory _inventory;
    [SerializeField] private StatesOfPlayer _stateOfPlayer;
    private IDash _dash;
    private IAttack _attack;
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
        _dash = GetComponent<IDash>();
        _attack = GetComponent<IAttack>();
    }

    private void Update()
    {
        if (_timerIntervalForAttack <= _intervalForAttack)
        {
            _timerIntervalForAttack += Time.deltaTime;
        }

        ActionChoice();
    }

    private void ActionChoice()
    {
        var axis = Vector3.zero;
        if (Input.GetKey(KeyCode.D)) axis.x++;
        if (Input.GetKey(KeyCode.A)) axis.x--;
        if (Input.GetKey(KeyCode.W)) axis.y++;
        if (Input.GetKey(KeyCode.S)) axis.y--;

        var isAttack = Input.GetKeyDown(KeyCode.E)
                       &&
                       _timerIntervalForAttack > _intervalForAttack;
        var isDash = Input.GetKeyDown(KeyCode.Space);

        var isInteraction = Input.GetKeyDown(KeyCode.I);

        switch (_stateOfPlayer)
        {
            case StatesOfPlayer.Idle or StatesOfPlayer.Move:
                if (isAttack)
                {
                    _timerIntervalForAttack -= _intervalForAttack;
                    _attack.Attack();
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
                    return;
                }

                _dash.Dash();
                break;
            case StatesOfPlayer.Attack:
                if (_attack.StateOfAttack == StatesOfAttack.Idle)
                {
                    _stateOfPlayer = StatesOfPlayer.Idle;
                    return;
                }

                _attack.Attack();
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
                    Debug.Log($"{_firstname} обратился к {_interaction?.FirstName}");
                    _interaction?.Interact(this);
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