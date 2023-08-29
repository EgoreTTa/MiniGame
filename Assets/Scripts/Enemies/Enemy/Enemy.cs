namespace Assets.Scripts.Enemies.Enemy
{
    using System;
    using System.Linq;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using JetBrains.Annotations;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    public class Enemy : BaseMob, IHealthSystem
    {
        public enum StatesOfEnemy
        {
            Idle,
            Explore,
            Pursuit,
            Attack
        }

        [SerializeField] private StatesOfEnemy _stateOfEnemy = StatesOfEnemy.Idle;
        [SerializeField] private BaseMob _targetToAttack;
        [SerializeField] private Vector3? _targetToExplore;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _distanceToAttack;
        private IMoveSystem _moveSystem;
        private IAttack _attack;

        public StatesOfEnemy StateOfEnemy => _stateOfEnemy;

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

                if (value >= _maxHealth)
                {
                    value = _maxHealth;
                }

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
            else throw new Exception($"{nameof(Enemy)} not instance {nameof(IMoveSystem)}");
            if (GetComponentInChildren<IAttack>() is { } iAttack) _attack = iAttack;
            else throw new Exception($"{nameof(Enemy)} not instance {nameof(IAttack)}");
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
                        transform.position + transform.up * .75f);

                    if (distanceToTarget < _distanceToAttack)
                    {
                        _stateOfEnemy = StatesOfEnemy.Attack;
                        _attack.Attack();
                        break;
                    }

                    Pursuit();
                    break;
                case StatesOfEnemy.Attack:
                    if (_attack.StateOfAttack == StatesOfAttack.Idle)
                    {
                        _stateOfEnemy = StatesOfEnemy.Idle;
                    }

                    break;
                default:
                    throw new Exception($"{nameof(Enemy)} FSM: not valid state");
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
    }
}