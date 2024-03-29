namespace Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class DefaultHelmet : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private TypesEquipment _typeEquipment;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => EquipmentSets.None;

        public void Equip()
        {
            _owner.Attribute.HealthPointsMaximum += _changeMaxHealth;
        }

        public void Unequip()
        {
            _owner.Attribute.HealthPointsMaximum -= _changeMaxHealth;
        }
    }
}