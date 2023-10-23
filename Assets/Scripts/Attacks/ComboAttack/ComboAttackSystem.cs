namespace Assets.Scripts.Attacks.ComboAttack
{
    using Mobs;
    using Enums;
    using Interfaces;
    using UnityEngine;

    public class ComboAttackSystem : BaseAttackSystem
    {
        private bool _isConstruct;
        private int _lengthCombination;
        private int _maxLengthCombination;
        [SerializeField] private float _timeSwing;
        [SerializeField] private float _timeHitting;
        [SerializeField] private float _timeRecovering;
        [SerializeField] private StatesOfAttack _stateOfAttack;
        [SerializeField] private BaseHit[] _comboHits;
        [SerializeField] private float _damageCount;
        [SerializeField] private float _damagePercent;

        public override StatesOfAttack StateOfAttack => _stateOfAttack;

        public override float DamageCount
        {
            get => _damageCount;
            set => _damageCount = value > 0 ? value : 0;
        }

        public override float DamagePercent
        {
            get => _damagePercent;
            set => _damagePercent = value;
        }

        public override BaseAttackSystem Construct(
            BaseMob owner,
            GroupsMobs ownerGroupsMobs,
            BaseHealthSystem ownerHealthSystem,
            Transform ownerTransform)
        {
            if (_isConstruct is false)
            {
                _maxLengthCombination = _comboHits.Length;

                for (var i = 0; i < _comboHits.Length; i++)
                {
                    if (_comboHits[i].GetComponent<BaseHit>() is { } hit)
                        _comboHits[i] = hit.Construct(owner, ownerGroupsMobs, ownerHealthSystem);
                    else
                        Debug.LogError($"{nameof(ComboAttackSystem)} not instance {nameof(BaseHit)}");
                }

                _isConstruct = true;
                return this;
            }

            return null;
        }

        public override void Attack()
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
            _comboHits[_lengthCombination].Hit(_damageCount + _damageCount * _damagePercent / 100);
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