using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Inventory : INotifyPropertyChanged
{
    private Player _owner;
    
    [SerializeField, Sirenix.OdinInspector.ReadOnly] private List<ItemSlot> _itemSlots;
    public ItemSlot this[int i]
    {
        get
        {
            if (_itemSlots.Count <= i)
            {
                return null;
            }
            return _itemSlots[i];
        }
    }

    private int _size;
    public int Size => _size;
    
    public Inventory(int size, Player player)
    {
        _size = size;
        _itemSlots = new List<ItemSlot>(size);
        _owner = player;
    }

    public bool AddItem(Item item)
    {
        if (item == null) return false;

        foreach (var itemSlot in _itemSlots)
        {
            if (itemSlot?.item == item)
            {
                bool result = itemSlot.Add();

                if (result)
                {
                    OnPropertyChanged("_itemSlots");
                    return true;
                }
            }
        }

        if (_itemSlots.Count < _size)
        {
            _itemSlots.Add(new ItemSlot(item));
            
            OnPropertyChanged("_itemSlots");
            return true;
        }
        
        return false;
    }

    public bool UseItem(ItemSlot slot)
    {
        if (slot == null) return false;
        
        slot.item?.Use(_owner);
        slot.Remove();
        
        OnPropertyChanged("_itemSlots");
        return true;
    }

    #region Fields:Notify

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
    
}