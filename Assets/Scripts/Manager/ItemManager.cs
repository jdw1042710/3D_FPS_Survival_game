using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct ItemEffect
{
    public string name; // 아이템 이름
    public enum EffectType
    {
        HPUp,
        SPUp,
        DPUp,
        HungryUp,
        ThirstyUp,
        SatisfyUp,
    }
    [Tooltip("적용할 효과들")]
    public EffectType[] types;
    [Tooltip("해당 효과들의 수치(개수가 일치해야함!)")]
    public int[] counts;
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    
    [SerializeField]
    private ItemEffect[] effectDB;

    [SerializeField]
    private StatusController _statusController;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public void UseItem(Item _item)
    {
        switch (_item.itemType)
        {
            case Item.ItemType.Equipment:
                WeaponManager.instance.TryChangeWeapon(WeaponManager.WeaponType.Axe, _item.itemName);
                break;
            case Item.ItemType.Used:
                for (int i = 0; i < effectDB.Length; i++)
                {
                    if (effectDB[i].name.Equals(_item.name))
                    {
                        AdjustEffect(effectDB[i]);
                        return;
                    }
                }
                Debug.Log("해당 아이템 효과를 찾을 수 없습니다.");
                break;
        }
    }

    private void AdjustEffect(ItemEffect _itemEffect)
    {
        for (int i = 0; i < _itemEffect.types.Length; i++)
        {
            switch (_itemEffect.types[i])
            {
                case ItemEffect.EffectType.HPUp:
                    _statusController.IncreaseHP(_itemEffect.counts[i]);
                    break;
                case ItemEffect.EffectType.SPUp:
                    _statusController.IncreaseSP(_itemEffect.counts[i]);
                    break;
                case ItemEffect.EffectType.DPUp:
                    _statusController.IncreaseDP(_itemEffect.counts[i]);
                    break;
                case ItemEffect.EffectType.HungryUp:
                    _statusController.IncreaseHungry(_itemEffect.counts[i]);
                    break;
                case ItemEffect.EffectType.ThirstyUp:
                    _statusController.IncreaseThirsty(_itemEffect.counts[i]);
                    break;
                case ItemEffect.EffectType.SatisfyUp:
                    _statusController.IncreaseSatisfy(_itemEffect.counts[i]);
                    break;
            }
        }
    }
}
