using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Game.Character;
using UnityEngine;

[Serializable]
public class Inventory : INotifyPropertyChanged
{
    private Character _owner;
    
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

    #region Methods:Ctor

    public Inventory(int size, Character character)
    {
        _size = size;
        _itemSlots = new List<ItemSlot>(size);
        _owner = character;
    }
    
    #endregion

    #region Methods:Inventory

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
        
        slot.Use(_owner);
        return true;
    }

    #endregion

    #region Fields:Notify

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
    
}