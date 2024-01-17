namespace GUI
{
    using System;
    using Abilities;
    using Enums;
    using Interfaces;
    using Items;
    using Mobs.Player;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ManagerGUI : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private GameObject _inGameMenu;
        [SerializeField] private StatesGame _stateGame;
        [SerializeField] private AbilityGUI _abilityGUI1;
        [SerializeField] private AbilityGUI _abilityGUI2;
        [SerializeField] private AbilityGUI _abilityGUI3;
        [SerializeField] private AbilityGUI _abilityGUI4;

        public StatesGame StateGame
        {
            get => _stateGame;
            private set
            {
                switch (value)
                {
                    case StatesGame.Pause:
                        _inGameMenu.SetActive(true);
                        Time.timeScale = 0;
                        break;
                    case StatesGame.Game:
                        _inGameMenu.SetActive(false);
                        Time.timeScale = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }

                _stateGame = value;
            }
        }

        private void Start()
        {
            StateGame = StatesGame.Game;
        }

        private void Update()
        {
            var isEscapeKey = Input.GetKeyDown(KeyCode.Escape);

            if (isEscapeKey)
            {
                InGameMenuActivation();
            }
        }

        public void InGameMenuActivation()
        {
            StateGame = _stateGame switch
            {
                StatesGame.Game => StatesGame.Pause,
                StatesGame.Pause => StatesGame.Game,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void UpdateHealthBar(float health, float maxHealth)
        {
            _healthBar.UpdateBar(health, maxHealth);
        }

        public void SetAbility(Sprite spriteAbility, string nameAbility, string hintTypingAbility, string descriptionAbility)
        {
            _abilityGUI1.SetAbility(spriteAbility, nameAbility, hintTypingAbility, descriptionAbility);
        }

        public void UpdateAbilityReload(float amount, float time)
        {
            _abilityGUI1.UpdateAbilityReload(amount, time);
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