namespace Assets.Scripts.Attacks
{
    using System;
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    public class DefaultAttackSystem : MonoBehaviour, IAttackSystem
    {
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        private BaseMob _owner;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception("Default not instance BaseMob");
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
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
            Invoke(nameof(IntoRecovery), _timeHitting);
        }

        private void IntoRecovery()
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;
            _stateOfAttack = StatesOfAttack.Recovery;
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<IHealthSystem>() is { } healthSystem)
            {
                if (collider.gameObject != _owner.gameObject)
                {
                    var damage = new Damage(_owner, null, _owner.DamageCount, TypesDamage.Clear);
                    healthSystem.TakeDamage(damage);
                }
            }
        }
    }
}