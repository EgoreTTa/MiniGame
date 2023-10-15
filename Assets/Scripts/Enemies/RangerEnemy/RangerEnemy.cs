namespace Assets.Scripts.Enemies.RangerEnemy
{
    using Interfaces;
    using Enums;
    using UnityEngine;
    using System;
    using JetBrains.Annotations;
    using Random = UnityEngine.Random;
    using System.Linq;

    public class RangerEnemy : MonoBehaviour, IMob
    {
        public enum StatesOfRangerEnemy
        {
            Idle,
            Explore,
            Pursuit,
            Attack
        }

        private IHealthSystem _healthSystem;
        private IMovementSystem _movementSystem;
        private IAttackSystem _attackSystem;
        private IMob _targetToAttack;
        private Vector3? _targetToExplore;
        [SerializeField] private StatesOfRangerEnemy _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;

        public string FirstName => _firstname;
        public GroupsMobs GroupMobs => _groupMobs;
        public IHealthSystem HealthSystem => _healthSystem;
        public IMovementSystem MovementSystem => _movementSystem;
        public IAttackSystem AttackSystem => _attackSystem;
        public StatesOfRangerEnemy StateOfRangerEnemy => _stateOfRangerEnemy;

        public IMob TargetToAttack
        {
            get => _targetToAttack;
            set
            {
                if ((value as MonoBehaviour)!.gameObject.activeSelf) _targetToAttack = value;
            }
        }

        private void Awake()
        {
            if (GetComponent<IHealthSystem>() is { } healthSystem)
                _healthSystem = healthSystem.Construct();
            else
                Debug.LogError($"{nameof(RangerEnemy)} not instance {nameof(IMovementSystem)}");

            if (GetComponent<IMovementSystem>() is { } moveSystem)
                _movementSystem = moveSystem.Construct(transform);
            else
                Debug.LogError($"{nameof(RangerEnemy)} not instance {nameof(IMovementSystem)}");

            if (GetComponentInChildren<IAttackSystem>() is { } attackSystem)
                _attackSystem = attackSystem.Construct(this, _groupMobs, _healthSystem, transform);
            else
                Debug.LogError($"{nameof(RangerEnemy)} not instance {nameof(IAttackSystem)}");

            _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
            Invoke(nameof(IntoExplore), _timeForIdle);
        }

        [UsedImplicitly]
        private void Update()
        {
            LookAround();
            ActionChoice();
        }

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
                        (_targetToAttack as MonoBehaviour)!.transform.position,
                        transform.position);

                    if (distanceToTarget < _distanceToAttack)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Attack;
                        transform.up = (_targetToAttack as MonoBehaviour)!.transform.position - transform.position;
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
                    throw new Exception($"{nameof(RangerEnemy)} FSM: not valid {nameof(StateOfRangerEnemy)}");
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
                MoveToPosition((_targetToAttack as MonoBehaviour)!.transform.position);
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

        private IMob[] GetMobsForRadius(float radius)
        {
            var casted = Physics2D.CircleCastAll(
                transform.position,
                radius,
                Vector2.zero);
            var mobs = casted
                .Where(x =>
                    x.transform.GetComponent<IMob>() is { } mob
                    &&
                    ReferenceEquals(mob, this) is false
                    &&
                    mob.GroupMobs != _groupMobs)
                .Select(x => x.transform.GetComponent<IMob>())
                .Distinct()
                .ToArray();
            return mobs;
        }
    }
}