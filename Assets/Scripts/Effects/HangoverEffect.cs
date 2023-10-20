namespace Assets.Scripts.Effects
{
    using Mobs;
    using Enums;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class HangoverEffect : BaseEffect
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _intervalTakeDamage;
        [SerializeField] private float _damageCount;
        private BaseMob _target;
        private bool _isActive;

        public override string EffectName => nameof(HangoverEffect);

        public override bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        public void StartEffect(BaseMob target)
        {
            InvokeRepeating(nameof(TakeDamage), 0, _intervalTakeDamage);
        }

        public void PostponeEffect()
        {
            CancelInvoke(nameof(TakeDamage));
            Invoke(nameof(StartEffect), _delay);
        }

        private void TakeDamage()
        {
            var damage = new Damage(null, null, _damageCount, TypesDamage.Clear);
            _target.HealthSystem?.TakeDamage(damage);
        }

        private void Awake()
        {
            _target = GetComponent<BaseMob>();
            if (_target == null)
                Destroy(this);
            else
                Invoke(nameof(StartEffect), _delay);
        }
    }
}