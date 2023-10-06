namespace Assets.Scripts.Attacks
{
    using System;
    using Enemies.RangerEnemy;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultRangeAttackSystem : MonoBehaviour, IAttackSystem
    {
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
        private BaseMob _owner;
        private SpriteRenderer _spriteRenderer;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        private void Ready()
        {
            _isReady = true;
        }

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception($"{nameof(DefaultRangeAttackSystem)} not instance {nameof(BaseMob)}");
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
            Invoke(nameof(IntoRecovery), _timeHitting);
        }

        private void IntoRecovery()
        {
            _spriteRenderer.enabled = false;
            _stateOfAttack = StatesOfAttack.Recovery;
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
        }

        private void BottleThrow()
        {
            var directionThrow = transform.up.normalized;
            var bottle = Instantiate(_bottle, _owner.transform.position, Quaternion.identity).GetComponent<Bottle>();

            if (bottle != null)
            {
                var projectile = bottle.GetComponent<IProjectile>();
                var bottleDamage = new Damage(_owner, null, _damageCount, TypesDamage.Clear);
                projectile.Launch(_bottleSpeed, bottleDamage, directionThrow, _bottleFlyTime, _owner);
            }
        }
    }
}