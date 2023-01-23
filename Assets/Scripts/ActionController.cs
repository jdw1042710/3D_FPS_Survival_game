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
    private LayerMask layerMask;
    [SerializeField] 
    private TextMeshProUGUI notificationText; //상호작용 가능할 시 활성화될 텍스트

    private void Awake()
    {
        notificationText.gameObject.SetActive(false);
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
        actionAvailable = Physics.Raycast(transform.position, transform.forward, out hitInfo, range,layerMask);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);
        if (!actionAvailable)
        {
            DisappearItemInfo();
            return;
        };
        
        ItemController _itemController = hitInfo.transform.GetComponent<ItemController>();
        if (_itemController)
        {
            AppearItemInfo(_itemController.item.itemName);
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
        ItemController _itemController = hitInfo.transform.GetComponent<ItemController>();
        if (_itemController)
        {
            Debug.Log($"{_itemController.item.itemName}을 획득하였습니다.");
            Destroy(_itemController.gameObject);
        }
    }
}
