namespace Interfaces
{
    using Enums;

    public interface IEquipment
    {
        public TypesEquipment TypeEquipment { get; }
        public EquipmentSets EquipmentSet { get; }

        public void Equip();
        public void Unequip();
    }
}