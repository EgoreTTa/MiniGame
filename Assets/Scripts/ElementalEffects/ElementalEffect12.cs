namespace Assets.Scripts.ElementalEffects
{
    using Enums;
    using Interfaces;
    using NoMonoBehaviour;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class ElementalEffect12 : BaseElementalEffect
    {
        [SerializeField] private float _damageCount = 4f;
        [SerializeField] private float _timeOfAction = 15f;
        [SerializeField] private float _intervalDamage = .1f;
        [SerializeField] private float _changeMoveSpeed = 1.5f;

        private void Start()
        {
            _typeElement = TypesElement.ElementalEffect12;
            if (_target is IMoveSystem moveSystem)
            {
                moveSystem.MoveSpeed -= _changeMoveSpeed;
                Destroy(this, _timeOfAction);
                InvokeRepeating(nameof(MakeDamage), 0, _intervalDamage);
            }
            else
                Destroy(this);
        }

        private void OnDestroy()
        {
            if (_target is IMoveSystem moveSystem)
                moveSystem.MoveSpeed += _changeMoveSpeed;
        }

        private void MakeDamage()
        {
            var damage = new Damage(
                _owner,
                null,
                _damageCount,
                TypesDamage.Clear);
            if (_target is IHealthSystem healthSystem)
                healthSystem.TakeDamage(damage);
        }
    }
}