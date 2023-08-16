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

    public enum StatesOfPlayer
    {
        Idle,
        Move,
        Dash,
        Attack,
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
            default:
                throw new Exception("FSM of Player: not valid state");
        }
    }

    protected override void DecreaseHealth()
    {
        Scorer.ClearStreak();
    }
}