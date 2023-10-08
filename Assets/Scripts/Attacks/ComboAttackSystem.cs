namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class ComboAttackSystem : MonoBehaviour, IAttackSystem
    {
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private float _damageCount;
        private int _lengthCombination;
        private int _maxLengthCombination;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        public float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        private void Awake()
        {
            _maxLengthCombination = transform.childCount - 1;
        }

        public void Attack()
        {
            var isContinueCombo = _stateOfAttack is StatesOfAttack.Recovery
                                  &&
                                  _lengthCombination <= _maxLengthCombination;
            if (_stateOfAttack is StatesOfAttack.Idle
                ||
                isContinueCombo)
            {
                if (_lengthCombination > 0)
                    CancelInvoke(nameof(IntoIdle));

                _stateOfAttack = StatesOfAttack.Swing;
                Invoke(nameof(IntoHitting), _timeSwing);
            }
        }

        private void IntoHitting()
        {
            transform.GetChild(_lengthCombination).gameObject.SetActive(true);
            _stateOfAttack = StatesOfAttack.Hitting;
            Invoke(nameof(IntoRecovery), _timeHitting);
        }

        private void IntoRecovery()
        {
            transform.GetChild(_lengthCombination).gameObject.SetActive(false);
            _lengthCombination++;
            _stateOfAttack = StatesOfAttack.Recovery;
            Invoke(nameof(IntoIdle), _timeRecovery);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
            _lengthCombination = 0;
        }
    }
}