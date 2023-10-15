namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [RequireComponent(typeof(CircleCollider2D))]
    public class DefaultAttackSystem : MonoBehaviour, IAttackSystem
    {
        private bool _isConstruct;
        private IMob _owner;
        private GroupsMobs _ownerGroupMobs;
        private IHealthSystem _ownerHealthSystem;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private float _damageCount;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        public float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
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
                _ownerGroupMobs = ownerGroupsMobs;
                _ownerHealthSystem = ownerHealthSystem;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void Attack()
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
            if (collider.GetComponent<IHealthSystem>() is { } healthSystem
                &&
                healthSystem != _ownerHealthSystem)
            {
                if (collider.GetComponent<IMob>() is { } mob
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