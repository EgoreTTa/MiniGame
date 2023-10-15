namespace Assets.Scripts.Enemies.Enemy
{
    using System;
    using System.Linq;
    using Enums;
    using Interfaces;
    using JetBrains.Annotations;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    public class Enemy : MonoBehaviour, IMob
    {
        public enum StatesOfEnemy
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
        [SerializeField] private StatesOfEnemy _stateOfEnemy = StatesOfEnemy.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;
        [SerializeField] private float _viewRadius;

        public string FirstName => _firstname;
        public GroupsMobs GroupMobs => _groupMobs;
        public IHealthSystem HealthSystem => _healthSystem;
        public IMovementSystem MovementSystem => _movementSystem;
        public IAttackSystem AttackSystem => _attackSystem;
        public StatesOfEnemy StateOfEnemy => _stateOfEnemy;

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
                Debug.LogError($"{nameof(Enemy)} not instance {nameof(IHealthSystem)}");

            if (GetComponent<IMovementSystem>() is { } moveSystem)
                _movementSystem = moveSystem.Construct(transform);
            else
                Debug.LogError($"{nameof(Enemy)} not instance {nameof(IMovementSystem)}");

            if (GetComponentInChildren<IAttackSystem>() is { } attackSystem)
                _attackSystem = attackSystem.Construct(this, _groupMobs, _healthSystem, transform);
            else
                Debug.LogError($"{nameof(Enemy)} not instance {nameof(IAttackSystem)}");

            _stateOfEnemy = StatesOfEnemy.Idle;
            Invoke(nameof(IntoExplore), _timeForIdle);
        }

        [UsedImplicitly]
        private void Update()
        {
            LookAround();
            ActionChoice();
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
                        (_targetToAttack as MonoBehaviour)!.transform.position,
                        (_attackSystem as MonoBehaviour)!.gameObject.transform.position);

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
                    throw new Exception($"{nameof(StateOfEnemy)} of {nameof(Enemy)}: not valid state");
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
    }
}