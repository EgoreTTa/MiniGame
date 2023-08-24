namespace Assets.Scripts.NoMonoBehaviour
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;
    using Items;

    public class Inventory
    {
        private List<BaseItem> _items = new();
        private IEquipment _helmet;
        private IEquipment _armor;
        private IEquipment _boot;
        private Player _parent;

        public BaseItem[] Items => _items.ToArray();

        public Inventory(Player parent)
        {
            _parent = parent;
        }

        public void Put(BaseItem item)
        {
            _items.Add(item);
            item.PickUp(_parent);
        }

        public void Take(BaseItem item)
        {
            _items.Remove(item);
            item.PickDown(_parent);
        }

        public void Equip(IEquipment equipment)
        {
            switch (equipment.TypeEquipment)
            {
                case TypesEquipment.Helmet:
                    if (_helmet != null) Unequip(_helmet);
                    _helmet = equipment;

                    break;
                case TypesEquipment.Armor:
                    if (_armor != null) Unequip(_armor);
                    _armor = equipment;

                    break;
                case TypesEquipment.Boot:
                    if (_boot != null) Unequip(_boot);
                    _boot = equipment;

                    break;
                default:
                    throw new Exception("Error Equip");
            }

            equipment.Equip();
        }

        public void Unequip(IEquipment equipment)
        {
            switch (equipment.TypeEquipment)
            {
                case TypesEquipment.Helmet:
                    _helmet = null;

                    break;
                case TypesEquipment.Armor:
                    _armor = null;

                    break;
                case TypesEquipment.Boot:
                    _boot = null;

                    break;
                default:
                    throw new Exception("Error Unequip");
            }

            equipment.Unequip();
        }
    }
}