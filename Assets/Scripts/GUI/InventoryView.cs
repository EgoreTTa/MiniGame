using Assets.Scripts.GUI;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private ManagerGUI _mGUI;
    [SerializeField] private GameObject[] _items;
    [SerializeField] private GameObject _helmet;
    [SerializeField] private GameObject _armor;
    [SerializeField] private GameObject _boots;

    public void GetItems()
    {
        _helmet.transform.GetChild(0).GetComponent<Image>().sprite = _mGUI.GetHelmet().SpriteRenderer.sprite;
        _armor.transform.GetChild(0).GetComponent<Image>().sprite = _mGUI.GetArmor().SpriteRenderer.sprite;
        _boots.transform.GetChild(0).GetComponent<Image>().sprite = _mGUI.GetBoot().SpriteRenderer.sprite;
        var Items = _mGUI.GetItemsInventory();

        for (var i = 0; i < Items.Length; i++)
        {
            _items[i].transform.GetChild(0).GetComponent<Image>().sprite = Items[i].SpriteRenderer.sprite;
        }
    }
}