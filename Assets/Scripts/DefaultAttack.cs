using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BaseMob))]
public class DefaultAttack : MonoBehaviour, IAttack
{
    [SerializeField] private float _timeSwing;
    private float _timerSwing;
    [SerializeField] private float _timeHitting;
    private float _timerHitting;
    [SerializeField] private float _timeRecovery;
    private float _timerRecovery;
    [SerializeField] private float _offsetForAttack;
    [SerializeField] private float _radiusForAttack;
    [SerializeField] private StatesOfAttack _stateOfAttack;
    private BaseMob _mob;
    private readonly List<BaseMob> _affectedTargets = new();

    public StatesOfAttack StateOfAttack => _stateOfAttack;

    private void Awake()
    {
        _mob = GetComponent<BaseMob>();
    }

    public void Attack()
    {
        if (_stateOfAttack != StatesOfAttack.Idle) return;

        if (_mob.Stamina > 0)
        {
            _mob.Stamina--;
            _stateOfAttack = StatesOfAttack.Swing;
        }
    }

    private void Hitting()
    {
        if (_timerHitting <= _timeHitting)
        {
            _timerHitting += Time.deltaTime;
            Hit();
        }
    }

    private void Hit()
    {
        var triggerPosition =
            transform.position +
            _mob.Direction * _offsetForAttack;

        var mobs = GetMobsForRadius(triggerPosition, _radiusForAttack);

        if (mobs.Length > 0)
            foreach (var hit in mobs)
                if (_affectedTargets.Contains(hit) is false)
                {
                    var damage = new Damage(_mob, null, TypesDamage.Clear, _mob.DamageCount);
                    hit.TakeDamage(damage);
                    _affectedTargets.Add(hit);

                    break;
                }
    }

    private BaseMob[] GetMobsForRadius(Vector3 zero, float radius)
    {
        var casted = Physics2D.CircleCastAll(
            zero,
            radius,
            Vector2.zero);
        var mobs = casted
            .Where(x =>
                x.transform.GetComponent<BaseMob>()
                &&
                x.transform.GetComponent<BaseMob>() != this)
            .Distinct()
            .Select(x => x.transform.GetComponent<BaseMob>())
            .ToArray();
        return mobs;
    }

    private void Update()
    {
        if (_stateOfAttack == StatesOfAttack.Idle) return;
        ActionChoice();
    }

    private void ActionChoice()
    {
        switch (_stateOfAttack)
        {
            case StatesOfAttack.Idle:
                break;
            case StatesOfAttack.Swing:
                if (_timerSwing > _timeSwing)
                {
                    _timerSwing -= _timeSwing;
                    _stateOfAttack = StatesOfAttack.Hitting;
                    return;
                }

                Swing();
                break;
            case StatesOfAttack.Hitting:
                if (_timerHitting > _timeHitting)
                {
                    _timerHitting -= _timeHitting;
                    _affectedTargets.Clear();
                    _stateOfAttack = StatesOfAttack.Recovery;
                    return;
                }

                Hitting();
                break;
            case StatesOfAttack.Recovery:
                if (_timerRecovery > _timeRecovery)
                {
                    _timerRecovery -= _timeRecovery;
                    _stateOfAttack = StatesOfAttack.Idle;
                    return;
                }

                Recovery();
                break;
            default:
                throw new Exception("FSM of DefaultAttack: not valid state");
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
}