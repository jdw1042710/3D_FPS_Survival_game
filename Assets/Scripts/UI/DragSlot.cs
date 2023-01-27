using System.ComponentModel;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    
    [ReadOnly(false)]
    public Slot slot;

    private Image itemImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Init()
    {
        itemImage = GetComponent<Image>();
        itemImage.enabled = false;
    }
    
    public void SetDragSlot(Slot _slot)
    {
        slot = _slot;
        itemImage.enabled = true;
        itemImage.sprite = _slot.itemImage.sprite;
    }
    
    public void ClearDragSlot()
    {
        slot = null;
        itemImage.enabled = false;
    }
}
