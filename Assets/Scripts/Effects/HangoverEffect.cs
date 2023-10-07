namespace Assets.Scripts.Effects
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class HangoverEffect : BaseEffect
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _intervalTakeDamage;
        [SerializeField] private float _damageCount;
        private IMob _target;

        public override void StartEffect(IMob target)
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
            (_target as MonoBehaviour)!.GetComponent<IHealthSystem>().TakeDamage(damage);
        }

        private void Awake()
        {
            _target = GetComponent<IMob>();
            if (_target == null)
                Destroy(this);
            else
                Invoke(nameof(StartEffect), _delay);
        }
    }
}