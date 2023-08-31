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
    }
}