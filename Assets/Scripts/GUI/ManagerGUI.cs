namespace GUI
{
    using System;
    using Abilities;
    using Enums;
    using Interfaces;
    using Items;
    using Mobs;
    using Mobs.Player;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class ManagerGUI : MonoBehaviour, ITakeDamage, ITakeHealth, IAttributeChange
    {
        [SerializeField] private Player _player;
        [SerializeField] private AttributeGUI _attributeGUI;
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
            _player.HealthSystem.Subscribe(this as ITakeHealth);
            _player.HealthSystem.Subscribe(this as ITakeDamage);
            _healthBar.UpdateBar(_player.HealthSystem.Health, _player.HealthSystem.MaxHealth);
            _player.Attribute.Subscribe(this);
            _attributeGUI.AttributeChange(_player.Attribute);
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

        public void SetAbility(BaseAbility ability)
        {
            _abilityGUI1.SetAbility(ability);
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

        public void TakeDamage(BaseHealthSystem healthSystem)
        {
            _healthBar.UpdateBar(healthSystem.Health, healthSystem.MaxHealth);
        }

        public void TakeHealth(BaseHealthSystem healthSystem)
        {
            _healthBar.UpdateBar(healthSystem.Health, healthSystem.MaxHealth);
        }

        public void AttributeChange(AttributeMob attributeMob)
        {
            _attributeGUI.AttributeChange(attributeMob);
        }
    }
}