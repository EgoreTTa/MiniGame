namespace Assets.Scripts.Ability.AbilityThrowGrenade
{
    using Abilities;
    using Mobs;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;
    using Attacks.DefaultRangeAttack;
    using Grenade;

    public class AbilityThrowGrenade : BaseAbility
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GameObject _ownerGameObject;
        private Transform _ownerTransform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private StatesOfAbility _stateOfAbility;
        [SerializeField] private float _timeToSwing;
        [SerializeField] private float _timeToCasted;
        [SerializeField] private float _timeToRecovery;
        [SerializeField] private GameObject _grenede;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _grenedeSpeed;
        [SerializeField] private float _grenadeFlyTime;
        [SerializeField] private float _timeReload;
        [SerializeField] private float _timeExplosion;
        [SerializeField] private float _damageCountExplosion;
        [SerializeField] private GameObject _explosion;
        [SerializeField] private bool _isReady;

        [SerializeField]
        private float _damagePercent;

        public override StatesOfAbility StateOfAbility => _stateOfAbility;

        public override BaseAbility Construct(BaseMob owner, GroupsMobs ownerGroupMobs, GameObject ownerGameObject)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGameObject = ownerGameObject;
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

        public override void Cast()
        {
            if (_stateOfAbility != StatesOfAbility.Standby
                ||
                _isReady is false) return;

            _isReady = false;
            Invoke(nameof(Ready), _timeReload);
            Swing();
        }

        private void Swing()
        {
            _stateOfAbility = StatesOfAbility.Swing;
            Invoke(nameof(IntoCasted), _timeToSwing);
        }

        private void Throw()
        {
            var directionThrow = _ownerGameObject.transform.up.normalized;
            var Grenade = Instantiate(_grenede, _ownerGameObject.transform.position, Quaternion.identity)
                .GetComponent<BaseProjectile>();

            if (Grenade != null)
            {
                var GrenadeDamage = new Damage(_owner, null, _damageCount, TypesDamage.Clear);
                var projectile = Grenade.GetComponent<BaseProjectile>();
                projectile.Launch(_grenedeSpeed, GrenadeDamage, directionThrow, _grenadeFlyTime, _owner);
                Grenade.GetComponent<Grenade>().SetExplosion(_timeExplosion, _damageCountExplosion, _explosion);
            }
        }
    }
}