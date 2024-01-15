namespace Attacks.ComboAttack
{
    using Enums;
    using Mobs;
    using NoMonoBehaviour;
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DefaultComboBaseHit : BaseHit
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GroupsMobs _ownerGroupMobs;
        private BaseHealthSystem _ownerHealthSystem;
        private float _damageCount;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _collider2D;

        public override float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        public override BaseHit Construct(
            BaseMob owner,
            GroupsMobs ownerGroupMobs,
            BaseHealthSystem ownerHealthSystem)
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

        public override void Hit(float ownerDamageCount)
        {
            _spriteRenderer.enabled = true;
            _collider2D.enabled = true;
            _damageCount = ownerDamageCount;
        }

        public override void Recovery()
        {
            _spriteRenderer.enabled = false;
            _collider2D.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<BaseHealthSystem>() is { } healthSystem
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