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
    }
}