namespace Mobs.Enemies.Kamikaze
{
    using System;
    using System.Linq;
    using Attacks;
    using Enums;
    using Interfaces;
    using JetBrains.Annotations;
    using Movements;
    using NoMonoBehaviour;
    using UnityEngine;
    using Random = UnityEngine.Random;

    [DisallowMultipleComponent]
    public class Kamikaze : BaseMob
    {
        public enum StatesOfKamikaze
        {
            Idle,
            Explore,
            Pursuit,
            Detonation
        }

        [SerializeField] private BaseHealthSystem _healthSystem;
        [SerializeField] private BaseMovement _movementSystem;
        private Vector3? _targetToExplore;
        [SerializeField] private StatesOfKamikaze _stateOfKamikaze = StatesOfKamikaze.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private BaseMob _targetToAttack;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _timeForDetonation;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _detonationRadius;
        [SerializeField] private Rigidbody2D _rigidbody;

        public override string FirstName => _firstname;
        public override GroupsMobs GroupMobs => _groupMobs;
        public override BaseHealthSystem HealthSystem => _healthSystem;
        public override BaseMovement MovementSystem => _movementSystem;
        public override BaseAttackSystem AttackSystem => null;
        public StatesOfKamikaze StateOfKamikaze => _stateOfKamikaze;
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

            _stateOfKamikaze = StatesOfKamikaze.Idle;
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

        private void Explosion()
        {
            if (_healthSystem.IsLive)
            {
                var mobs = GetMobsForRadius(_explosionRadius);
                var healthSystems = mobs
                    .Select(x =>
                        x.HealthSystem)
                    .ToArray();

                var damage = new Damage(this, null, _damageCount, TypesDamage.Clear);
                foreach (var healthSystem in healthSystems) healthSystem?.TakeDamage(damage);
                _healthSystem.TakeDamage(new Damage(this, null, _healthSystem.MaxHealth, TypesDamage.Clear));
            }
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

        private void IntoExplore()
        {
            _stateOfKamikaze = StatesOfKamikaze.Explore;
            FindPositionToExplore();
        }

        private void ActionChoice()
        {
            switch (_stateOfKamikaze)
            {
                case StatesOfKamikaze.Idle:
                    if (_targetToAttack != null)
                    {
                        _stateOfKamikaze = StatesOfKamikaze.Pursuit;
                        CancelInvoke(nameof(IntoExplore));
                        break;
                    }

                    break;
                case StatesOfKamikaze.Explore:
                    if (_targetToAttack != null)
                    {
                        _stateOfKamikaze = StatesOfKamikaze.Pursuit;
                        break;
                    }

                    if (_targetToExplore == null)
                    {
                        _stateOfKamikaze = StatesOfKamikaze.Idle;
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    Explore();
                    break;
                case StatesOfKamikaze.Pursuit:
                    if (_targetToAttack == null)
                    {
                        _stateOfKamikaze = StatesOfKamikaze.Idle;
                        Invoke(nameof(IntoExplore), _timeForIdle);
                        break;
                    }

                    var distanceToTarget = Vector3.Distance(
                        _targetToAttack.transform.position,
                        transform.position);

                    if (distanceToTarget < _detonationRadius)
                    {
                        _stateOfKamikaze = StatesOfKamikaze.Detonation;
                        Invoke(nameof(Explosion), _timeForDetonation);
                        break;
                    }

                    Pursuit();
                    break;
                case StatesOfKamikaze.Detonation:
                    break;
                default:
                    throw new Exception($"{nameof(Kamikaze)} FSM: not valid {nameof(StateOfKamikaze)}");
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