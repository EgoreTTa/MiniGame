namespace Assets.Scripts.Attacks
{
    using System;
    using Enemies.RangerEnemy;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultRangeAttack : MonoBehaviour, IAttack
    {
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private GameObject _bottle;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _bottleSpeed;
        private BaseMob _owner;
        private SpriteRenderer _spriteRenderer;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception($"{nameof(DefaultRangeAttack)} not instance {nameof(BaseMob)}");
            _spriteRenderer = GetComponent<SpriteRenderer>();
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
                var bottleDamage = new Damage(_owner, null, _damageCount, TypesDamage.Clear);
                bottle.Direction = directionThrow;
                bottle.Damage = bottleDamage;
                bottle.Speed = _bottleSpeed;
                bottle.Parent = _owner.gameObject;
                Destroy(bottle.GetComponent<Bottle>(), 5f);
            }
        }
    }
}