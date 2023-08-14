using System;
using UnityEngine;

[RequireComponent(typeof(BaseMob))]
public class DefaultDash : MonoBehaviour, IDash
{
    [SerializeField] private float _timeSwing;
    private float _timerSwing;
    [SerializeField] private float _timeMoving;
    private float _timerMoving;
    [SerializeField] private float _timeRecovery;
    private float _timerRecovery;
    [SerializeField] private float _speedMoving;

    [SerializeField] private StatesOfDash _stateOfDash;
    private BaseMob _mob;

    public StatesOfDash StateOfDash => _stateOfDash;

    private void Awake() { _mob = GetComponent<BaseMob>(); }

    private void Moving()
    {
        if (_timerMoving <= _timeMoving)
        {
            _timerMoving += Time.deltaTime;
            Move();
        }
    }

    private void Move()
    {
        _mob.transform.position += _mob.Direction * _speedMoving * Time.deltaTime;
    }

    private void Update()
    {
        if (_stateOfDash == StatesOfDash.Idle) return;
        ActionChoice();
    }

    private void ActionChoice()
    {
        switch (_stateOfDash)
        {
            case StatesOfDash.Idle:
                break;
            case StatesOfDash.Swing:
                if (_timerSwing > _timeSwing)
                {
                    _timerSwing -= _timeSwing;
                    _mob.GetComponent<CircleCollider2D>().enabled = false;
                    _stateOfDash = StatesOfDash.Moving;
                    return;
                }

                Swing();
                break;
            case StatesOfDash.Moving:
                if (_timerMoving > _timeMoving)
                {
                    _timerMoving -= _timeMoving;
                    _mob.GetComponent<CircleCollider2D>().enabled = true;
                    _stateOfDash = StatesOfDash.Recovery;
                    return;
                }

                Moving();
                break;
            case StatesOfDash.Recovery:
                if (_timerRecovery > _timeRecovery)
                {
                    _timerRecovery -= _timeRecovery;
                    _stateOfDash = StatesOfDash.Idle;
                    return;
                }

                Recovery();
                break;
            default:
                throw new Exception("FSM: not valid state");
        }
    }

    private void Recovery()
    {
        if (_timerRecovery <= _timeRecovery) _timerRecovery += Time.deltaTime;
    }

    private void Swing()
    {
        if (_timerSwing <= _timeSwing) _timerSwing += Time.deltaTime;
    }

    public void Dash()
    {
        if (_stateOfDash != StatesOfDash.Idle) return;

        _stateOfDash = StatesOfDash.Swing;
    }
}