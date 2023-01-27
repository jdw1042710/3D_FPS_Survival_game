using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool activated
    {
        get;
        private set;
    } = false;
    
    [SerializeField]
    private GameObject InventoryUI;
    [SerializeField] 
    private GameObject slotsParent;

    private Slot[] slots;

    private void Awake()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
        InventoryUI.SetActive(false);
    }

    private void Update()
    {
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activated = !activated;
            if (activated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }

    }

    private void OpenInventory()
    {
        PlayerController.isActivated = false;
        WeaponManager.instance.DisableController();
        InventoryUI.SetActive(true);
    }

    private void CloseInventory()
    {
        PlayerController.isActivated = true;
        WeaponManager.instance.EnableController();
        InventoryUI.SetActive(false);
    }

    public bool AddItem(Item _item, int _count = 1)
    {
        if (_item.itemType == Item.ItemType.Equipment)
            return AddNewItem(_item, _count);
        
        return AddExistItem(_item, _count);
    }

    private bool AddExistItem(Item _item, int _count)
    {
        //기존 아이템에 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item && slots[i].item.itemName == _item.itemName)
            {
                slots[i].itemCount += _count;
                return true;
            }
        }
        // 새로운 슬롯에 추가
        return AddNewItem(_item, _count);
    }
    
    private bool AddNewItem(Item _item, int _count)
    {
        // 새로운 슬롯에 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].item)
            {
                slots[i].SetItem(_item, _count);
                return true;
            }
        }
        // 아이템 획득 실패
        return false;
    }
}
