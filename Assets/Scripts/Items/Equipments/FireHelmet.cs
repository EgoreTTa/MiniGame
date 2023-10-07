namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;
    using NoMonoBehaviour;

    [DisallowMultipleComponent]
    public class FireHelmet : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private float _changeRegeneration;
        [SerializeField] private TypesEquipment _typeEquipment;
        [SerializeField] private float _intervalRegeneration;
        private IHealthSystem _healthSystem;
        private Health _health;

        public TypesEquipment TypeEquipment => _typeEquipment;

        private void Regeneration()
        {
            _healthSystem.TakeHealth(_health);
        }

        public void Equip()
        {
            _healthSystem = _owner.GetComponent<IHealthSystem>();
            _healthSystem.MaxHealth += _changeMaxHealth;
            _health = new Health(_owner, gameObject, _changeRegeneration);
            InvokeRepeating(nameof(Regeneration), 0f, _intervalRegeneration);
        }

        public void Unequip()
        {
            _healthSystem.MaxHealth -= _changeMaxHealth;
            CancelInvoke(nameof(Regeneration));
        }
    }
}