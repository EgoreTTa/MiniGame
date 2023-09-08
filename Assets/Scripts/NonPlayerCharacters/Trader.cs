namespace Assets.Scripts.NonPlayerCharacters
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Items;
    using JetBrains.Annotations;
    using NoMonoBehaviour;
    using UnityEngine;

    [UsedImplicitly]
    public class Trader : NonPlayerCharacter, IInteraction, ITrading
    {
        [SerializeField] private List<BaseItem> _items;

        public string FirstName => _firstName;
        public bool IsInteract => _isInteract;

        public void Interact(Player player)
        {
            if (_isInteract == false) _isInteract = true;

            if (_items.Any())
            {
                Trade(_items.First(), player.Inventory);
            }
        }

        public void Trade(BaseItem item, Inventory inventory)
        {
            if (inventory.Currency >= item.Currency)
            {
                if (_items.Contains(item))
                {
                    _items.Remove(item);
                    inventory.Put(item);
                    inventory.Currency -= item.Currency;
                }
            }
        }
    }
}