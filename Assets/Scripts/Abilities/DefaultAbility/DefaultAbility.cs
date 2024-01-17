namespace Abilities.DefaultAbility
{
    using ElementalEffects;
    using Enums;
    using GUI;
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
        [SerializeField] private float _timerReload;

        public override StatesOfAbility StateOfAbility => _stateOfAbility;

        public override BaseAbility Construct(BaseMob owner, GroupsMobs ownerGroupMobs, GameObject ownerGameObject,
            ManagerGUI managerGUI)
        {
            if (_isConstruct is false)
            {
                _owner = owner;
                _ownerGameObject = ownerGameObject;
                _isConstruct = true;
                _managerGUI = managerGUI;
                _managerGUI.SetAbility(_spriteAbility, _nameAbility, _typingAbility, _descriptionAbility);
                return this;
            }

            return null;
        }

        private void Update()
        {
            if (_isReady is false)
            {
                _timerReload += Time.deltaTime;
                if (_timerReload > _timeReload)
                {
                    _timerReload = _timeReload;
                    _isReady = true;
                }

                _managerGUI?.UpdateAbilityReload(1 - _timerReload / _timeReload, _timeReload - _timerReload);
            }
        }

        private void IntoSwing()
        {
            _stateOfAbility = StatesOfAbility.Swing;
            Invoke(nameof(IntoCasted), _timeToSwing);
        }

        private void IntoCasted()
        {
            _spriteRenderer.enabled = true;
            _circleCollider.enabled = true;
            _isReady = false;
            _timerReload = 0;
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

            IntoSwing();
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