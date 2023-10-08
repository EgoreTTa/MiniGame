namespace Assets.Scripts.GUI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class InventoryView : MonoBehaviour
    {
        [SerializeField] private ManagerGUI _mGUI;
        [SerializeField] private Image[] _items;
        [SerializeField] private Image _helmet;
        [SerializeField] private Image _armor;
        [SerializeField] private Image _boots;

        public void GetItems()
        {
            _helmet.sprite = _mGUI.GetHelmet().SpriteRenderer.sprite;
            _armor.sprite = _mGUI.GetArmor().SpriteRenderer.sprite;
            _boots.sprite = _mGUI.GetBoot().SpriteRenderer.sprite;
            var Items = _mGUI.GetItemsInventory();

            for (var i = 0; i < Items.Length; i++)
                _items[i].sprite = Items[i].SpriteRenderer.sprite;
        }
    }
}