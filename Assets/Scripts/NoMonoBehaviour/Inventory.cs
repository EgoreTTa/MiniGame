namespace Assets.Scripts.NoMonoBehaviour
{
    using System;
    using System.Collections.Generic;
    using Enums;
    using Interfaces;
    using Items;
    using Mobs.Player;

    public class Inventory
    {
        private readonly List<BaseItem> _items = new();
        private IEquipment _helmet;
        private IEquipment _armor;
        private IEquipment _boot;
        private readonly Player _owner;
        private float _currency;

        public BaseItem[] Items => _items.ToArray();
        public BaseItem Helmet => _helmet as BaseItem;
        public BaseItem Armor => _armor as BaseItem;
        public BaseItem Boot => _boot as BaseItem;

        public float Currency
        {
            get => _currency;
            set
            {
                if (value < 0) value = 0;
                _currency = value;
            }
        }

        public Inventory(Player owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Adds an item to inventory
        /// </summary>
        public void Put(BaseItem item)
        {
            _items.Add(item);
            item.PickUp(_owner);
        }

        /// <summary>
        /// Removes an item from inventory
        /// </summary>
        public void Take(BaseItem item)
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                item.PickDown(_owner);
            }
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