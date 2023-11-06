namespace Assets.Scripts.Abilities.ThrowGrenadeAbility
{
    using Abilities;
    using Mobs;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;
    using Projectiles;

    public class ThrowGrenadeAbility : BaseAbility
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GroupsMobs _ownerGroupMobs;
        private GameObject _ownerGameObject;
        private Transform _ownerTransform;
        private Vector3 _positionCast;
        private Vector3 _directionCast;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private StatesOfAbility _stateOfAbility;
        [SerializeField] [Min(.2f)] private float _timeToSwing;
        [SerializeField] [Min(.2f)] private float _timeToCasted;
        [SerializeField] [Min(.2f)] private float _timeToRecovery;
        [SerializeField] private GameObject _grenade;
        [SerializeField] [Min(0)] private float _damageCount;
        [SerializeField] [Min(.2f)] private float _grenadeSpeed;
        [SerializeField] private float _grenadeFlyTime;
        [SerializeField] [Min(.2f)] private float _timeReload;
        [SerializeField] [Min(0)] private float _damageCountExplosion;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private bool _isReady;

        public override StatesOfAbility StateOfAbility => _stateOfAbility;

        public override BaseAbility Construct(BaseMob owner, GroupsMobs ownerGroupMobs, GameObject ownerGameObject)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGameObject = ownerGameObject;
                _ownerGroupMobs = ownerGroupMobs;
                _isConstruct = true;
                return this;
            }

            return null;
        }

        private void Ready()
        {
            _isReady = true;
        }

        private void IntoCasted()
        {
            _stateOfAbility = StatesOfAbility.Casted;
            _spriteRenderer.enabled = true;
            Throw();
            Invoke(nameof(IntoRecovery), _timeToCasted);
        }

        private void IntoRecovery()
        {
            _stateOfAbility = StatesOfAbility.Recovery;
            _spriteRenderer.enabled = false;
            Invoke(nameof(IntoStandby), _timeToRecovery);
        }

        private void IntoStandby()
        {
            _stateOfAbility = StatesOfAbility.Standby;
        }

        public override void Cast(Vector3? position, Vector3? direction)
        {
            if (_stateOfAbility != StatesOfAbility.Standby
                ||
                _isReady is false) return;

            _isReady = false;
            Invoke(nameof(Ready), _timeReload);
            _positionCast = position!.Value;
            _directionCast = direction!.Value.normalized;
            Swing();
        }

        private void Swing()
        {
            _stateOfAbility = StatesOfAbility.Swing;
            Invoke(nameof(IntoCasted), _timeToSwing);
        }

        private void Throw()
        {
            var grenadeDamage = new Damage(_owner, gameObject, _damageCount, TypesDamage.Clear);
            var grenade = Instantiate(_grenade, _positionCast, Quaternion.identity);
            var projectile = grenade.GetComponent<BaseProjectile>().Construct(
                _grenadeSpeed,
                grenadeDamage,
                _directionCast,
                _grenadeFlyTime,
                _owner,
                _ownerGroupMobs,
                _ownerGameObject);
            ((Grenade)projectile).SetExplosion(_damageCountExplosion, _explosion);
        }
    }
}