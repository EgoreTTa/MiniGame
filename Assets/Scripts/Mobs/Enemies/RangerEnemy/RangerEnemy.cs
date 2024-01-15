namespace Mobs.Enemies.RangerEnemy
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

    public class RangerEnemy : BaseMob
    {
        public enum StatesOfRangerEnemy
        {
            Idle,
            Explore,
            Pursuit,
            Attack
        }

        private BaseMob _targetToAttack;
        private Vector3? _targetToExplore;
        [SerializeField] private BaseHealthSystem _healthSystem;
        [SerializeField] private BaseMovement _movementSystem;
        [SerializeField] private BaseAttackSystem _attackSystem;
        [SerializeField] private StatesOfRangerEnemy _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;
        [SerializeField] private Rigidbody2D _rigidbody;

        public override string FirstName => _firstname;
        public override GroupsMobs GroupMobs => _groupMobs;
        public override BaseHealthSystem HealthSystem => _healthSystem;
        public override BaseMovement MovementSystem => _movementSystem;
        public override BaseAttackSystem AttackSystem => _attackSystem;
        public StatesOfRangerEnemy StateOfRangerEnemy => _stateOfRangerEnemy;

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

            _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
            Invoke(nameof(IntoExplore), _timeForIdle);
        }

        [UsedImplicitly]
        private void Update()
        {
            LookAround();
            ActionChoice();
        }

        public override void KilledMob(BaseMob mob) { }
        public override void Subscribe(IKillerMob killerMob) { }
        public override void Unsubscribe(IKillerMob killerMob) { }

        private void IntoExplore()
        {
            _stateOfRangerEnemy = StatesOfRangerEnemy.Explore;
            FindPositionToExplore();
        }

        private void ActionChoice()
        {
            switch (_stateOfRangerEnemy)
            {
                case StatesOfRangerEnemy.Idle:
                    if (_targetToAttack != null)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Pursuit;
                        CancelInvoke(nameof(IntoExplore));
                        break;
                    }

                    break;
                case StatesOfRangerEnemy.Explore:
                    if (_targetToAttack != null)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Pursuit;
                        break;
                    }

                    if (_targetToExplore == null)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    Explore();
                    break;
                case StatesOfRangerEnemy.Pursuit:
                    if (_targetToAttack == null)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    var distanceToTarget = Vector3.Distance(
                        _targetToAttack.transform.position,
                        transform.position);

                    if (distanceToTarget < _distanceToAttack)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Attack;
                        transform.up = _targetToAttack!.transform.position - transform.position;
                        _attackSystem.Attack();
                        break;
                    }

                    Pursuit();
                    break;
                case StatesOfRangerEnemy.Attack:
                    if (_attackSystem.StateOfAttack == StatesOfAttack.Idle)
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;

                    break;
                default:
                    throw new Exception($"{nameof(RangerEnemy)} FSM: not valid {nameof(StatesOfRangerEnemy)}");
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
    }
}