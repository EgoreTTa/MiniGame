namespace Assets.Scripts.NonPlayerCharacters
{
    using System.Linq;
    using Interfaces;
    using Items;
    using JetBrains.Annotations;
    using NoMonoBehaviour;
    using UnityEngine;

    [UsedImplicitly]
    public class Trader : NonPlayerCharacter, IInteraction, ITrading
    {
        private Inventory _inventory;

        public string FirstName => _firstName;
        public bool IsInteract => _isInteract;

        public void Interact(Player player)
        {
            if (_isInteract == false) _isInteract = true;

            Debug.Log($"{_firstName} торгует {player.Firstname}");
            if (_inventory.Items.Any())
            {
                Trade(_inventory.Items.First(), player.Inventory);
            }
        }

        public void Trade(BaseItem item, Inventory inventory)
        {
            if (inventory.Currency >= item.Currency)
            {
                if (_inventory.Items.Contains(item))
                {
                    _inventory.Take(item);
                    _inventory.Currency += item.Currency;
                    inventory.Put(item);
                    inventory.Currency -= item.Currency;
                }
            }
        }
    }
}