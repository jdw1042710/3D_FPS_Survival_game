using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    private RectTransform baseRectTransform;
    
    [SerializeField] private GameObject uiBase;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Text itemUsingGuideText;
    private void Awake()
    {
        baseRectTransform = uiBase.GetComponent<RectTransform>();

        uiBase.SetActive(false);
    }

    public void Show(Item _item, Vector3 _position)
    {
        uiBase.SetActive(true);
        
        Rect _rect = baseRectTransform.rect;
        _position += new Vector3(_rect.width * 0.5f, -_rect.height * 0.5f, 0);
        transform.position = _position;
        itemNameText.text = _item.itemName;
        itemDescriptionText.text = _item.description;
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment:
                itemUsingGuideText.text = "우클릭-장착";
                break;
            case Item.ItemType.Used:
                itemUsingGuideText.text = "우클릭-사용";
                break;
            default:
                itemUsingGuideText.text = "";
                break;
        }
    }

    public void Hide()
    {
        uiBase.SetActive(false);
    }
}
