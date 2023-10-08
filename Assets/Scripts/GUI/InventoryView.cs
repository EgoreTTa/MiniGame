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
          //  if (_mGUI == null) return;
            _helmet.sprite = _mGUI.GetHelmet()?.SpriteRenderer.sprite;
            _armor.sprite = _mGUI.GetArmor()?.SpriteRenderer.sprite;
            _boots.sprite = _mGUI.GetBoot()?.SpriteRenderer.sprite;
            var items = _mGUI.GetItemsInventory();

            for (var i = 0; i < items.Length; i++)
                _items[i].sprite = items[i].SpriteRenderer.sprite;
        }
    }
}