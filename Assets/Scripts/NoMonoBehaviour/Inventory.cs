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
        public BaseItem Helmet => _helmet as BaseItem;
        public BaseItem Armor => _armor as BaseItem;
        public BaseItem Boot => _boot as BaseItem;

        public Inventory(Player parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Adds an item to inventory
        /// </summary>
        public void Put(BaseItem item)
        {
            _items.Add(item);
            item.PickUp(_parent);
        }

        /// <summary>
        /// Removes an item from inventory
        /// </summary>
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
                    if (_helmet != null) UnEquip(_helmet);
                    _helmet = equipment;

                    break;
                case TypesEquipment.Armor:
                    if (_armor != null) UnEquip(_armor);
                    _armor = equipment;

                    break;
                case TypesEquipment.Boot:
                    if (_boot != null) UnEquip(_boot);
                    _boot = equipment;

                    break;
                default:
                    throw new Exception("Error Equip");
            }

            _items.Remove(equipment as BaseItem);
            equipment.Equip();
        }

        public void UnEquip(IEquipment equipment)
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
                    throw new Exception("Error UnEquip");
            }

            _items.Add(equipment as BaseItem);
            equipment.Unequip();
        }
    }
}