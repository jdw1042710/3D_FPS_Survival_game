using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public Image itemImage;
    private int _itemCount;
    public int itemCount
    {
        get =>  this._itemCount;
        set
        {
            this._itemCount = value;
            if(_itemCount == 0) Clear();
            countText.text = _itemCount.ToString();
        }
    }

    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private GameObject go_itemImage;
    [SerializeField]
    private GameObject go_countImage;
    

    //슬롯에 아이템 등록
    public void SetItem(Item _item, int _count = 1)
    {
        item = _item;
        go_itemImage.SetActive(true);
        if (item.itemType == Item.ItemType.Equipment)
        {
            go_countImage.SetActive(false);
        }
        else
        {
            go_countImage.SetActive(true);
            itemCount = _count;
        }

        itemImage.sprite = item.itemImage;
    }

    //슬롯 비우기
    public void Clear()
    {
        item = null;
        _itemCount = 0;
        itemImage.sprite = null;
        go_itemImage.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            if (!item) return;
            ItemManager.instance.UseItem(item);
            if (item.itemType.Equals(Item.ItemType.Used)) itemCount--;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!item) return;

        DragSlot.instance.SetDragSlot(this);
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!item) return;
        DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(DragSlot.instance.slot)
            DragSlot.instance.ClearDragSlot();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.slot) 
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;
        
        SetItem(DragSlot.instance.slot.item, DragSlot.instance.slot.itemCount);

        if (_tempItem)
            DragSlot.instance.slot.SetItem(_tempItem, _tempItemCount);
        else
            DragSlot.instance.slot.Clear();
    }
}
