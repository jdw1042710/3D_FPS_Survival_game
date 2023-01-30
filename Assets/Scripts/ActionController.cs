using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] 
    private float range; // 상호작용 가능한 거리

    private bool actionAvailable = false; // 상호작용 가능 여부
    private RaycastHit hitInfo;

    [SerializeField]
    private Camera theCamera;
    [SerializeField] 
    private Inventory inventory;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField] 
    private TextMeshProUGUI notificationText; //상호작용 가능할 시 활성화될 텍스트

    private void Awake()
    {
        notificationText.gameObject.SetActive(false);
        theCamera = GetComponentInChildren<Camera>();
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        CheckItem();
        TryAction();
    }

    private void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }

    private void CheckItem()
    {
        actionAvailable =
            Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, range, layerMask);
        if (!actionAvailable)
        {
            DisappearItemInfo();
            return;
        };
        
        ItemProp _itemProp = hitInfo.transform.GetComponent<ItemProp>();
        if (_itemProp)
        {
            AppearItemInfo(_itemProp.item.itemName);
        }

    }

    private void AppearItemInfo(string _itemName)
    {
        notificationText.gameObject.SetActive(true);
        notificationText.text = $"Gain {_itemName} <color=yellow> (Press E) </color>";
    }
    
    private void DisappearItemInfo()
    {
        notificationText.gameObject.SetActive(false);
    }

    private void PickUp()
    {
        if (!actionAvailable) return;
        ItemProp _itemProp = hitInfo.transform.GetComponent<ItemProp>();
        if (_itemProp)
        {
            Debug.Log($"{_itemProp.item.itemName}을 획득하였습니다.");
            inventory.AddItem(_itemProp.item);
            Destroy(_itemProp.gameObject);
        }
    }
}
