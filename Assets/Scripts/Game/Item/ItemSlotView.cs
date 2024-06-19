using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ItemSlotView : MonoBehaviour
{
    public Image icon;
    [SerializeField] private TextMeshProUGUI count;
    public int index;
    
    [Inject] private Player _player;
    private ItemSlot itemSlot => _player.inventory[index];
    
    private void Start()
    {
        Init(itemSlot);
        _player.inventory.PropertyChanged += (sender, args) =>
        {
            Init(itemSlot);
        };
    }

    public void Init(ItemSlot slot)
    {
        if (slot == null)
        {
            icon.gameObject.SetActive(false);
            count.gameObject.SetActive(false);
            return;
        }
        
        // 껐으니깐 켜기
        icon.gameObject.SetActive(true);
        count.gameObject.SetActive(true);
        
        SetIcon(slot);
        SetCount(slot);
    }

    private void SetIcon(ItemSlot slot)
    {
        icon.sprite = slot.item.icon;
    }

    private void SetCount(ItemSlot slot)
    {
        count.text = slot.count.ToString();
    }
}
