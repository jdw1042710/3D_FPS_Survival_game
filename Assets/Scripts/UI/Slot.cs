using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item;
    private int _itemCount;
    public int itemCount
    {
        get =>  this._itemCount;
        set
        {
            this._itemCount = value;
            countText.text = _itemCount.ToString();
            if (_itemCount == 0)
                Clear();
        }
    }
    
    [SerializeField] 
    private Image itemImage;
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
        itemCount = 0;
        itemImage.sprite = null;
        gameObject.SetActive(false);
    }
}
