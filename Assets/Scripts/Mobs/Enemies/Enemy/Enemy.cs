namespace Mobs.Enemies.Enemy
{
    using System;
    using System.Linq;
    using Attacks;
    using Enums;
    using Interfaces;
    using JetBrains.Annotations;
    using Movements;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    public class Enemy : BaseMob
    {
        public enum StatesOfEnemy
        {
            Idle,
            Explore,
            Pursuit,
            Attack
        }

        [SerializeField] private BaseHealthSystem _healthSystem;
        [SerializeField] private BaseMovement _movementSystem;
        [SerializeField] private BaseAttackSystem _attackSystem;
        private BaseMob _targetToAttack;
        private Vector3? _targetToExplore;
        [SerializeField] private StatesOfEnemy _stateOfEnemy = StatesOfEnemy.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;
        [SerializeField] private float _viewRadius;
        [SerializeField] private Rigidbody2D _rigidbody;

        public override string FirstName => _firstname;
        public override GroupsMobs GroupMobs => _groupMobs;
        public override BaseHealthSystem HealthSystem => _healthSystem;
        public override BaseMovement MovementSystem => _movementSystem;
        public override BaseAttackSystem AttackSystem => _attackSystem;
        public StatesOfEnemy StateOfEnemy => _stateOfEnemy;
        public BaseMob TargetToAttack
        {
            get => _targetToAttack;
            set
            {
                if (value.gameObject.activeSelf) _targetToAttack = value;
            }
        }

        private void Awake()
        {
            _healthSystem.Construct(this);
            _movementSystem.Construct(transform, _rigidbody);
            _attackSystem.Construct(this, _groupMobs, _healthSystem, transform);

            _stateOfEnemy = StatesOfEnemy.Idle;
            Invoke(nameof(IntoExplore), _timeForIdle);
        }

        [UsedImplicitly]
        private void Update()
        {
            LookAround();
            ActionChoice();
        }

        private BaseMob[] GetMobsForRadius(float radius)
        {
            var casted = Physics2D.CircleCastAll(
                transform.position,
                radius,
                Vector2.zero);
            var mobs = casted
                .Where(x =>
                    x.transform.GetComponent<BaseMob>() is { } mob
                    &&
                    ReferenceEquals(mob, this) is false
                    &&
                    mob.GroupMobs != _groupMobs)
                .Select(x => x.transform.GetComponent<BaseMob>())
                .Distinct()
                .ToArray();
            return mobs;
        }

        public override void KilledMob(BaseMob mob) { }
        public override void Subscribe(IKillerMob killerMob) { }
        public override void Unsubscribe(IKillerMob killerMob) { }

        private void IntoExplore()
        {
            _stateOfEnemy = StatesOfEnemy.Explore;
            FindPositionToExplore();
        }

        private void ActionChoice()
        {
            switch (_stateOfEnemy)
            {
                case StatesOfEnemy.Idle:
                    if (_targetToAttack != null)
                    {
                        _stateOfEnemy = StatesOfEnemy.Pursuit;
                        CancelInvoke(nameof(IntoExplore));
                        break;
                    }

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
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    Explore();
                    break;
                case StatesOfEnemy.Pursuit:
                    if (_targetToAttack == null)
                    {
                        _stateOfEnemy = StatesOfEnemy.Idle;
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    var distanceToTarget = Vector3.Distance(
                        _targetToAttack.transform.position,
                        _attackSystem.gameObject.transform.position);

                    if (distanceToTarget < _distanceToAttack)
                    {
                        _stateOfEnemy = StatesOfEnemy.Attack;
                        _attackSystem.Attack();
                        break;
                    }

                    Pursuit();
                    break;
                case StatesOfEnemy.Attack:
                    if (_attackSystem.StateOfAttack == StatesOfAttack.Idle)
                    {
                        _stateOfEnemy = StatesOfEnemy.Idle;
                    }

                    break;
                default:
                    throw new Exception($"{nameof(StatesOfEnemy)} of {nameof(Enemy)}: not valid state");
            }
        }

        private void Explore()
        {
            var distanceToTarget = Vector3.Distance(
                _targetToExplore!.Value,
                transform.position);
            if (distanceToTarget < _movementSystem.MoveSpeed * Time.deltaTime)
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
            _movementSystem.Move(direction);
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
}