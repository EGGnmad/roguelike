using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ItemSlotView : MonoBehaviour
{
    public Image icon;
    [SerializeField] private TextMeshProUGUI count;
    public int offset;
    
    public void OnInventoryViewChanged(Inventory inventory)
    {
        int index = inventory.Index + offset;
        ItemSlot itemSlot = null;

        if (index < 0 || index > inventory.Size - 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        
        // 에러 핸들링
        if (index >= 0 && index < inventory.Size)
        {
            itemSlot = inventory[index];
        }
        
        SetIcon(itemSlot);
        SetCount(itemSlot);
    }
    
    private void SetIcon(ItemSlot slot)
    {
        if (slot == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }
        
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
    }

    private void SetCount(ItemSlot slot)
    {
        if (slot == null)
        {
            count.gameObject.SetActive(false);
            return;
        }
        
        count.gameObject.SetActive(true);
        count.text = slot.count.ToString();
    }
}
