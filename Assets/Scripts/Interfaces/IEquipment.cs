namespace Assets.Scripts.Interfaces
{
    using Enums;

    public interface IEquipment
    {
        public TypesEquipment TypeEquipment { get; }

        public void Equip();

        public void Unequip();
    }
}