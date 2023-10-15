namespace Assets.Scripts.Attacks
{
    using Enemies.RangerEnemy;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultRangeAttackSystem : MonoBehaviour, IAttackSystem
    {
        private bool _isConstruct;
        private IMob _owner;
        private Transform _ownerTransform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private GameObject _bottle;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _bottleSpeed;
        [SerializeField] private float _bottleFlyTime;
        [SerializeField] private float _timeReload;
        [SerializeField] private bool _isReady;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        public float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        private void Ready()
        {
            _isReady = true;
        }

        public IAttackSystem Construct(
            IMob owner,
            GroupsMobs ownerGroupsMobs,
            IHealthSystem ownerHealthSystem,
            Transform ownerTransform)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerTransform = ownerTransform;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void Attack()
        {
            if (_stateOfAttack != StatesOfAttack.Idle
                ||
                _isReady is false) return;

            _isReady = false;
            Invoke(nameof(Ready), _timeReload);
            _stateOfAttack = StatesOfAttack.Swing;
            Invoke(nameof(IntoHitting), _timeSwing);
        }

        private void IntoHitting()
        {
            _spriteRenderer.enabled = true;
            BottleThrow();
            _stateOfAttack = StatesOfAttack.Hitting;
            Invoke(nameof(IntoRecovering), _timeHitting);
        }

        private void IntoRecovering()
        {
            _spriteRenderer.enabled = false;
            _stateOfAttack = StatesOfAttack.Recovering;
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
        }

        private void BottleThrow()
        {
            var directionThrow = _ownerTransform.up.normalized;
            var bottle = Instantiate(_bottle, _ownerTransform.position, Quaternion.identity)
                .GetComponent<Bottle>();

            if (bottle != null)
            {
                var bottleDamage = new Damage(_owner, null, _damageCount, TypesDamage.Clear);
                var projectile = bottle.GetComponent<IProjectile>();
                projectile.Launch(_bottleSpeed, bottleDamage, directionThrow, _bottleFlyTime, _owner);
            }
        }
    }
}