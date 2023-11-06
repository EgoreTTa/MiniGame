namespace Assets.Scripts.Attacks.DefaultAttack
{
    using Mobs;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    [RequireComponent(typeof(CircleCollider2D))]
    public class DefaultAttackSystem : BaseAttackSystem
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GameObject _ownerGameObject;
        private GroupsMobs _ownerGroupMobs;
        private BaseHealthSystem _ownerHealthSystem;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _damagePercent;

        public override StatesOfAttack StateOfAttack => _stateOfAttack;

        public override float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        public override float DamagePercent
        {
            get => _damagePercent;
            set => _damagePercent = value;
        }

        public override BaseAttackSystem Construct(
            BaseMob owner,
            GroupsMobs ownerGroupsMobs,
            GameObject ownerGameObject,
            BaseHealthSystem ownerHealthSystem,
            Transform ownerTransform)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGroupMobs = ownerGroupsMobs;
                _ownerGameObject = ownerGameObject;
                _ownerHealthSystem = ownerHealthSystem;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public override void Attack()
        {
            if (_stateOfAttack != StatesOfAttack.Idle) return;

            _stateOfAttack = StatesOfAttack.Swing;
            Invoke(nameof(IntoHitting), _timeSwing);
        }

        private void IntoHitting()
        {
            _spriteRenderer.enabled = true;
            _circleCollider.enabled = true;
            _stateOfAttack = StatesOfAttack.Hitting;
            Invoke(nameof(IntoRecovering), _timeHitting);
        }

        private void IntoRecovering()
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;
            _stateOfAttack = StatesOfAttack.Recovering;
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.isTrigger is false
                &&
                collider.GetComponent<BaseHealthSystem>() is { } healthSystem
                &&
                healthSystem != _ownerHealthSystem)
            {
                if (collider.GetComponent<BaseMob>() is { } mob
                    &&
                    mob.GroupMobs == _ownerGroupMobs)
                {
                    return;
                }

                var damage = new Damage(_owner, gameObject, _damageCount, TypesDamage.Clear);
                healthSystem.TakeDamage(damage);
            }
        }
    }
}