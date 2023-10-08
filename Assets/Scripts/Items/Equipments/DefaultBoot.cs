namespace Assets.Scripts.Items.Equipments
{
    using Enums;
    using Interfaces;
    using UnityEngine;

    [DisallowMultipleComponent]
    public class DefaultBoot : BaseItem, IEquipment
    {
        [SerializeField] private float _changeMaxHealth;
        [SerializeField] private TypesEquipment _typeEquipment;

        public TypesEquipment TypeEquipment => _typeEquipment;
        public EquipmentSets EquipmentSet => EquipmentSets.None;

        public void Equip()
        {
            _owner.GetComponent<IHealthSystem>().MaxHealth += _changeMaxHealth;
        }

        public void Unequip()
        {
            _owner.GetComponent<IHealthSystem>().MaxHealth -= _changeMaxHealth;
        }
    }
}