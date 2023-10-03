namespace Assets.Scripts.Enemies.RangerEnemy
{
    using Interfaces;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;
    using System;
    using JetBrains.Annotations;
    using Random = UnityEngine.Random;
    using System.Linq;

    public class RangerEnemy : BaseMob, IHealthSystem
    {
        public enum StatesOfRangerEnemy
        {
            Idle,
            Explore,
            Pursuit,
            Attack
        }

        [SerializeField] private StatesOfRangerEnemy _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;
        [SerializeField] private BaseMob _targetToAttack;
        [SerializeField] private Vector3? _targetToExplore;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;
        private IMoveSystem _moveSystem;
        private IAttackSystem _attackSystem;

        public StatesOfRangerEnemy StateOfRangerEnemy => _stateOfRangerEnemy;

        public BaseMob TargetToAttack
        {
            get => _targetToAttack;
            set
            {
                if (value.gameObject.activeSelf) _targetToAttack = value;
            }
        }

        public float Health
        {
            get => _health;
            private set
            {
                if (value <= _minHealth)
                {
                    value = _minHealth;
                    _isLive = false;
                    Destroy(gameObject);
                }

                if (value >= _maxHealth) value = _maxHealth;

                _health = value;
            }
        }

        public float MinHealth
        {
            get => _minHealth;
            set
            {
                if (value <= 0) value = 0;
                if (value > _maxHealth) value = _maxHealth;
                _minHealth = value;
            }
        }

        public float MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (value <= _minHealth) value = _minHealth;
                _maxHealth = value;
            }
        }

        private void Awake()
        {
            if (GetComponent<IMoveSystem>() is { } iMoveSystem) _moveSystem = iMoveSystem;
            else throw new Exception($"{nameof(RangerEnemy)} not instance {nameof(IMoveSystem)}");
            if (GetComponentInChildren<IAttackSystem>() is { } iAttack) _attackSystem = iAttack;
            else throw new Exception($"{nameof(RangerEnemy)} not instance {nameof(IAttackSystem)}");
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
                        _targetToAttack.transform.position,
                        transform.position);

                    if (distanceToTarget < _distanceToAttack)
                    {
                        _stateOfRangerEnemy = StatesOfRangerEnemy.Attack;
                        transform.up = _targetToAttack.transform.position - transform.position;
                        _attackSystem.Attack();
                        break;
                    }

                    Pursuit();
                    break;
                case StatesOfRangerEnemy.Attack:
                    if (_attackSystem.StateOfAttack == StatesOfAttack.Idle) _stateOfRangerEnemy = StatesOfRangerEnemy.Idle;

                    break;
                default:
                    throw new Exception($"{nameof(RangerEnemy)} FSM: not valid state");
            }
        }

        private void Explore()
        {
            var distanceToTarget = Vector3.Distance(
                _targetToExplore!.Value,
                transform.position);
            if (distanceToTarget < _moveSystem.MoveSpeed * Time.deltaTime)
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
            _moveSystem.Move(direction);
        }

        public void TakeHealth(Health health)
        {
            Health += health.CountHealth;
        }

        public void TakeDamage(Damage damage)
        {
            Health -= damage.TypeDamage switch
            {
                TypesDamage.Physical => damage.CountDamage / 2,
                TypesDamage.Magical => damage.CountDamage * 2,
                TypesDamage.Clear => damage.CountDamage,
                _ => throw new ArgumentOutOfRangeException()
            };
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
                    x.transform.GetComponent<BaseMob>()
                    &&
                    x.transform.GetComponent<BaseMob>() != this
                    &&
                    x.transform.GetComponent<BaseMob>().GroupMobs != _groupMobs)
                .Select(x => x.transform.GetComponent<BaseMob>())
                .Distinct()
                .ToArray();
            return mobs;
        }
    }
}