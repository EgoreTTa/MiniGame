namespace Abilities.DefaultAbility
{
    using ElementalEffects;
    using Enums;
    using Mobs;
    using UnityEngine;

    public class DefaultAbility : BaseAbility
    {
        private bool _isConstruct;
        private BaseMob _owner;
        private GameObject _ownerGameObject;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private CircleCollider2D _circleCollider;
        [SerializeField] private StatesOfAbility _stateOfAbility;
        [SerializeField] private float _timeToSwing;
        [SerializeField] private float _timeToCasted;
        [SerializeField] private float _timeToRecovery;
        [SerializeField] private float _timeReload;
        [SerializeField] private bool _isReady;

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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<BaseMob>() is { } mob
                &&
                mob != _owner)
            {
                _ownerGameObject.AddComponent<ElementalEffect2>();
            }
        }
    }
}