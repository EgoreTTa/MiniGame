namespace Assets.Scripts.Effects
{
    using Enemies;
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class HangoverEffect : BaseEffect
    {
        [SerializeField] private float _delay;
        [SerializeField] private float _timeTakeDamage;
        [SerializeField] private bool _takingDamage;
        [SerializeField] private float _damage;
        private BaseMob _target;

        public override void StartEffect(BaseMob target)
        {
            _takingDamage = true;
            InvokeRepeating(nameof(TakeDamage), 0, _timeTakeDamage);
        }

        public void PostponeEffect()
        {
            _takingDamage = false;
            CancelInvoke(nameof(TakeDamage));
            Invoke(nameof(StartEffect), _delay);
        }

        private void TakeDamage()
        {
            var damage = new Damage(null, null, _damage, TypesDamage.Clear);
            (_target as IHealthSystem).TakeDamage(damage);
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