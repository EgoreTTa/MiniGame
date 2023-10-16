namespace Assets.Scripts.Attacks.DefaultRangeAttack
{
    using Mobs;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultRangeAttackSystem : BaseAttackSystem
    {
        private bool _isConstruct;
        private BaseMob _owner;
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

        public override StatesOfAttack StateOfAttack => _stateOfAttack;

        public override float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        private void Ready()
        {
            _isReady = true;
        }

        public override BaseAttackSystem Construct(
            BaseMob owner,
            GroupsMobs ownerGroupsMobs,
            BaseHealthSystem ownerHealthSystem,
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

        public override void Attack()
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
                .GetComponent<BaseProjectile>();

            if (bottle != null)
            {
                var bottleDamage = new Damage(_owner, null, _damageCount, TypesDamage.Clear);
                var projectile = bottle.GetComponent<BaseProjectile>();
                projectile.Launch(_bottleSpeed, bottleDamage, directionThrow, _bottleFlyTime, _owner);
            }
        }
    }
}