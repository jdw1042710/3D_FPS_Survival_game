using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Used,
        Ingredient,
        Etc,
        MaxCount,
    }
    
    public string itemName; //이름
    [TextArea]
    public string description; // 설명
    public ItemType itemType; //유형
    public Sprite itemImage; //이미지
    public GameObject itemPrefab; //프리펩


}
