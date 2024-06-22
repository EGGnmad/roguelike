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

    public int Size { get; private set; }
    public int Index { get; private set; }

    #region Methods:Ctor

    public Inventory(int size, Character character)
    {
        Size = size;
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

        if (_itemSlots.Count < Size)
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

    public void Prev()
    {
        Index = Mathf.Clamp(Index - 1, 0, Size - 1);
    }
    
    public void Next()
    {
        Index = Mathf.Clamp(Index + 1, 0, Size - 1);
    }

    public void UseItem()
    {
        UseItem(_itemSlots[Index]);
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