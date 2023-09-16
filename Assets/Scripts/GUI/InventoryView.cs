using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GUI;
using Assets.Scripts.Items;
using UnityEditor;
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
        _helmet.GetComponent<Image>().sprite = _mGUI.GetHelmet().SpriteRenderer.sprite;
        _armor.GetComponent<Image>().sprite = _mGUI.GetArmor().SpriteRenderer.sprite;
        _boots.GetComponent<Image>().sprite = _mGUI.GetBoot().SpriteRenderer.sprite;
        var Items = _mGUI.GetItemsInventory();

        for (var i = 0; i < Items.Length; i++)
        {
            _items[i].GetComponent<Image>().sprite = Items[i].SpriteRenderer.sprite;
            Debug.Log($"{i} {Items[i].name}");
        }
    }
}