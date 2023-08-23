namespace Assets.Scripts.Abilities
{
    using System;
    using ElementalEffects;
    using Enemies;
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class DefaultAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private StatesOfAbility _stateOfAbility;
        [SerializeField] private float _timeToSwing;
        [SerializeField] private float _timeToCasted;
        [SerializeField] private float _timeToRecovery;
        [SerializeField] private float _timeReload;
        private BaseMob _owner;
        [SerializeField] private bool _isReady;
        private SpriteRenderer _spriteRenderer;
        private CircleCollider2D _circleCollider;

        public StatesOfAbility StateOfAbility => _stateOfAbility;

        private void Awake()
        {
            if (GetComponentInParent<BaseMob>() is { } baseMob) _owner = baseMob;
            else throw new Exception("Default Ability not instance BaseMob");
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _circleCollider = GetComponent<CircleCollider2D>();
        }

        private void Ready()
        {
            _isReady = true;
        }

        private void IntoCasted()
        {
            _spriteRenderer.enabled = true;
            _circleCollider.enabled = true;
            _stateOfAbility = StatesOfAbility.Casted;
            Invoke(nameof(IntoRecovery), _timeToCasted);
        }

        private void IntoRecovery()
        {
            _spriteRenderer.enabled = false;
            _circleCollider.enabled = false;
            _stateOfAbility = StatesOfAbility.Recovery;
            Invoke(nameof(IntoStandby), _timeToRecovery);
        }

        private void IntoStandby()
        {
            _stateOfAbility = StatesOfAbility.Standby;
        }

        public void Cast()
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<BaseMob>() is { } baseMob)
            {
                if (baseMob != _owner)
                    baseMob.gameObject.AddComponent<ElementalEffect2>();
            }
        }
    }
}