namespace GUI
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
            if (_mGUI == null) return;
            if (_mGUI.GetHelmet() is { } helmet)
                _helmet.sprite = helmet.SpriteRenderer.sprite;
            if (_mGUI.GetArmor() is { } armor)
                _armor.sprite = armor.SpriteRenderer.sprite;
            if (_mGUI.GetBoot() is { } boots)
                _boots.sprite = boots.SpriteRenderer.sprite;

            var items = _mGUI.GetItemsInventory();
            for (var i = 0; i < items.Length; i++)
                _items[i].sprite = items[i].SpriteRenderer.sprite;
        }
    }
}