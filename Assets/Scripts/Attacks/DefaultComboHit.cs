namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DefaultComboHit : MonoBehaviour, IHit
    {
        private bool _isConstruct;
        private IMob _owner;
        private GroupsMobs _ownerGroupMobs;
        private IHealthSystem _ownerHealthSystem;
        private float _damageCount;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider2D;

        public float DamageCount
        {
            get => _damageCount;
            set
            {
                if (value < 0) value = 0;
                _damageCount = value;
            }
        }

        public IHit Construct(
            IMob owner,
            GroupsMobs ownerGroupMobs,
            IHealthSystem ownerHealthSystem)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGroupMobs = ownerGroupMobs;
                _ownerHealthSystem = ownerHealthSystem;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void Hit(float ownerDamageCount)
        {
            _spriteRenderer.enabled = true;
            _collider2D.enabled = true;
            _damageCount = ownerDamageCount;
        }

        public void Recovery()
        {
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
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