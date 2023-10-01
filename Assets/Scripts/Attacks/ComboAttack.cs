namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class ComboAttack : MonoBehaviour, IAttack
    {
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovery;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        private int _lengthCombination;
        private int _maxLengthCombination;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        private void Awake()
        {
            _maxLengthCombination = transform.childCount;
        }

        public void Attack()
        {
            var isNotContinueCombo = _stateOfAttack != StatesOfAttack.Recovery
                                  ||
                                  _lengthCombination > _maxLengthCombination;
            if (_stateOfAttack != StatesOfAttack.Idle
                &&
                isNotContinueCombo) return;

            if (_lengthCombination > 0)
                CancelInvoke(nameof(IntoIdle));

            _stateOfAttack = StatesOfAttack.Swing;
            Invoke(nameof(IntoHitting), _timeSwing);
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