using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[DisallowMultipleComponent]
public class Kamikaze : BaseMob
{
    public enum StatesOfEnemy
    {
        Idle,
        Explore,
        Pursuit,
        Detonation
    }

    [SerializeField] private int _score;
    [SerializeField] private StatesOfEnemy _stateOfEnemy = StatesOfEnemy.Idle;
    [SerializeField] private BaseMob _targetToAttack;
    [SerializeField] private Vector3? _targetToExplore;
    [SerializeField] private float _timeForIdle = default!;
    private float _timerTimeForIdle;
    [SerializeField] private float _timeForDetonation = default!;
    private float _timerTimeForDetonation;
    [SerializeField] private float _explosionRadius = default!;
    [SerializeField] private float _detonationRadius = default;

    public StatesOfEnemy StateOfEnemy => _stateOfEnemy;

    public BaseMob TargetToAttack
    {
        get => _targetToAttack;
        set
        {
            if (value.gameObject.activeSelf) _targetToAttack = value;
        }
    }

    [UsedImplicitly]
    private void Update()
    {
        LookAround();
        ActionChoice();
    }

    private void Detonation()
    {
        _timerTimeForDetonation += Time.deltaTime;
        if (_timerTimeForDetonation > _timeForDetonation) Explosion();
    }

    private void Explosion()
    {
        var mobs = GetMobsForRadius(_explosionRadius);

        var damage = new Damage(this, null, TypeDamage.Clear, _damageCount);
        foreach (var mob in mobs) mob.TakeDamage(damage);

        Destroy(gameObject);
    }

    private BaseMob[] GetMobsForRadius(float radius)
    {
        var casted = Physics2D.CircleCastAll(
            transform.position,
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

    private void ActionChoice()
    {
        switch (_stateOfEnemy)
        {
            case StatesOfEnemy.Idle:
                if (_targetToAttack != null)
                {
                    _stateOfEnemy = StatesOfEnemy.Pursuit;
                    break;
                }

                if (_timerTimeForIdle > _timeForIdle)
                {
                    _timerTimeForIdle -= _timeForIdle;
                    FindPositionToExplore();
                    _stateOfEnemy = StatesOfEnemy.Explore;
                    break;
                }

                Idle();
                break;
            case StatesOfEnemy.Explore:
                if (_targetToAttack != null)
                {
                    _stateOfEnemy = StatesOfEnemy.Pursuit;
                    break;
                }

                if (_targetToExplore == null)
                {
                    _stateOfEnemy = StatesOfEnemy.Idle;
                    break;
                }

                Explore();
                break;
            case StatesOfEnemy.Pursuit:
                if (_targetToAttack == null)
                {
                    _stateOfEnemy = StatesOfEnemy.Idle;
                    break;
                }

                var distanceToTarget = Vector3.Distance(
                    _targetToAttack.transform.position,
                    transform.position);

                if (distanceToTarget < _detonationRadius)
                {
                    _stateOfEnemy = StatesOfEnemy.Detonation;
                    break;
                }

                Pursuit();
                break;
            case StatesOfEnemy.Detonation:
                Detonation();
                break;
            default:
                throw new Exception("FSM: not valid state");
        }
    }

    private void Idle() { _timerTimeForIdle += Time.deltaTime; }

    private void Explore()
    {
        var distanceToTarget = Vector3.Distance(
            _targetToExplore!.Value,
            transform.position);
        if (distanceToTarget < _moveSpeed * Time.deltaTime)
            _targetToExplore = null;
        if (_targetToExplore != null)
            MoveToPosition(_targetToExplore.Value);
    }

    private void Pursuit()
    {
        if (_targetToAttack != null)
            MoveToPosition(_targetToAttack.transform.position);
    }

    private void MoveToPosition(Vector3 targetPosition)
    {
        var direction = targetPosition - transform.position;
        Walk(direction);
    }

    private void LookAround()
    {
        var mobs = GetMobsForRadius(_viewRadius);

        _targetToAttack = null;
        if (mobs.Any()) TargetToAttack = mobs.First();
    }

    private void FindPositionToExplore()
    {
        _targetToExplore = new Vector3(
            Random.Range(-5, 5),
            Random.Range(-5, 5));
    }
}