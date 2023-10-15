namespace Assets.Scripts.Attacks
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class ComboAttackSystem : MonoBehaviour, IAttackSystem
    {
        private bool _isConstruct;
        private int _lengthCombination;
        private int _maxLengthCombination;
        private IHit[] _comboHits;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovering;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private GameObject[] _gameObjectComboHits;
        [SerializeField] private float _damageCount;

        public StatesOfAttack StateOfAttack => _stateOfAttack;

        public float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        public IAttackSystem Construct(
            IMob owner,
            GroupsMobs ownerGroupsMobs,
            IHealthSystem ownerHealthSystem,
            Transform ownerTransform)
        {
            if (_isConstruct is false)
            {
                _maxLengthCombination = _gameObjectComboHits.Length;

                _comboHits = new IHit[_maxLengthCombination];
                for (var i = 0; i < _comboHits.Length; i++)
                {
                    if (_gameObjectComboHits[i].GetComponent<IHit>() is { } hit)
                        _comboHits[i] = hit.Construct(owner, ownerGroupsMobs, ownerHealthSystem);
                    else
                        Debug.LogError($"{nameof(ComboAttackSystem)} not instance {nameof(IHit)}");
                }

                _isConstruct = true;
                return this;
            }

            return null;
        }

        public void Attack()
        {
            var isContinueCombo = _stateOfAttack is StatesOfAttack.Recovering
                                  &&
                                  _lengthCombination < _maxLengthCombination;
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
            _comboHits[_lengthCombination].Hit(_damageCount);
            _stateOfAttack = StatesOfAttack.Hitting;
            Invoke(nameof(IntoRecovery), _timeHitting);
        }

        private void IntoRecovery()
        {
            _comboHits[_lengthCombination].Recovery();
            _lengthCombination++;
            _stateOfAttack = StatesOfAttack.Recovering;
            Invoke(nameof(IntoIdle), _timeRecovering);
        }

        private void IntoIdle()
        {
            _stateOfAttack = StatesOfAttack.Idle;
            _lengthCombination = 0;
        }
    }
}