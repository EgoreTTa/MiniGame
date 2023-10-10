namespace Assets.Scripts.GUI
{
    using Enums;
    using Interfaces;
    using Items;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ManagerGUI : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private HealthBar _healthBar;
        //[SerializeField] private Canvas _canvas;
        private StatesGame _stateGame;

        public StatesGame StateGame => _stateGame;

        private void Update()
        {
            var isEscapeKey = Input.GetKeyDown(KeyCode.Escape);
            if (isEscapeKey)
            {
                _stateGame = _stateGame switch
                {
                    StatesGame.Game => StatesGame.Pause,
                    StatesGame.Pause => StatesGame.Game,
                };
                Time.timeScale = (int)_stateGame;
                //_canvas.enabled = !_canvas.enabled;
            }
        }

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
            return _player.Inventory.Helmet;
        }

        public BaseItem GetArmor()
        {
            return _player.Inventory.Armor;
        }

        public BaseItem GetBoot()
        {
            return _player.Inventory.Boot;
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

        public void RemoveItem(BaseItem item)
        {
            _player.Inventory.Take(item);
        }

        public void ReturnMainMenu()
        {
            SceneManager.LoadSceneAsync(0);
        }
    }
}