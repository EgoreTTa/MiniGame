namespace Assets.Scripts.GUI
{
    using Interfaces;
    using Items;
    using UnityEngine;

    public class ManagerGUI : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private HealthBar _healthBar;

        public void UpdateHealthBar(float health, float maxHealth)
        {
            _healthBar.UpdateBar(health, maxHealth);
        }

        public BaseItem[] GetItemsInventory()
        {
            return _player.Inventory.Items;
        }

        public void UseItem(IUsable usable)
        {
            usable.Use(_player);
        }

        public BaseItem GetHelmet()
        {
            return _player.Inventory.Helmet as BaseItem;
        }

        public BaseItem GetArmor()
        {
            return _player.Inventory.Armor as BaseItem;
        }

        public BaseItem GetBoot()
        {
            return _player.Inventory.Boot as BaseItem;
        }

        public void SetHelmet(BaseItem helmet)
        {
            if (helmet is IEquipment equipment)
                _player.Inventory.Equip(equipment);
        }

        public void SetArmor(BaseItem armor)
        {
            if (armor is IEquipment equipment)
                _player.Inventory.Equip(equipment);
        }

        public void SetBoot(BaseItem boot)
        {
            if (boot is IEquipment equipment)
                _player.Inventory.Equip(equipment);
        }
    }
}