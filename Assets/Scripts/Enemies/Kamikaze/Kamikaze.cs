namespace Assets.Scripts.Enemies.Kamikaze
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
    public class Kamikaze : MonoBehaviour, IMob
    {
        public enum StatesOfKamikaze
        {
            Idle,
            Explore,
            Pursuit,
            Detonation
        }

        private IHealthSystem _healthSystem;
        private IMoveSystem _moveSystem;
        [SerializeField] private StatesOfKamikaze _stateOfKamikaze = StatesOfKamikaze.Idle;
        [SerializeField] private string _firstname;
        [SerializeField] private GroupsMobs _groupMobs;
        [SerializeField] private IMob _targetToAttack;
        [SerializeField] private Vector3? _targetToExplore;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _viewRadius;
        [SerializeField] private float _timeForIdle;
        [SerializeField] private float _timeForDetonation;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private float _detonationRadius;

        public string FirstName => _firstname;
        public GroupsMobs GroupMobs => _groupMobs;
        public StatesOfKamikaze StateOfKamikaze => _stateOfKamikaze;

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
            if (GetComponent<IHealthSystem>() is { } healthSystem) _healthSystem = healthSystem;
            else throw new Exception($"{nameof(Player)} not instance {nameof(IHealthSystem)}");
            if (GetComponent<IMoveSystem>() is { } moveSystem) _moveSystem = moveSystem;
            else throw new Exception($"{nameof(Kamikaze)} not instance {nameof(IMoveSystem)}");
            _stateOfKamikaze = StatesOfKamikaze.Idle;
            Invoke(nameof(IntoExplore), _timeForIdle);
        }

        [UsedImplicitly]
        private void Update()
        {
            LookAround();
            ActionChoice();
        }

        private void Explosion()
        {
            if (_healthSystem.IsLive)
            {
                var mobs = GetMobsForRadius(_explosionRadius);
                var healthSystems = mobs.Select(x =>
                        (x as MonoBehaviour)!.GetComponent<IHealthSystem>())
                    .ToArray();

                var damage = new Damage(this, null, _damageCount, TypesDamage.Clear);
                foreach (var healthSystem in healthSystems) healthSystem.TakeDamage(damage);
                _healthSystem.TakeDamage(new Damage(this, null, _healthSystem.MaxHealth, TypesDamage.Clear));
            }
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
                        (_targetToAttack as MonoBehaviour)!.transform.position,
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
            if (distanceToTarget < _moveSystem.MoveSpeed * Time.deltaTime)
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
            _moveSystem.Move(direction);
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