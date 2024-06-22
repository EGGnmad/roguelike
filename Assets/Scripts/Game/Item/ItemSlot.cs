using System;
using Game.Character;


[Serializable]
public class ItemSlot
{
    public Item item;
    public bool Empty => item == null;
    public int count;

    public ItemSlot()
    {
        item = null;
        count = 0;
    }
    
    public ItemSlot(Item item)
    {
        this.item = item;
        count = 1;
    }

    public bool Add()
    {
        if (count < item.stackSize)
        {
            count = count + 1;
            return true;
        }
        return false;
    }

    public bool Remove()
    {
        count = count - 1;
        if (count > 0) return true;
        return false;
    }

    public void Use(Character character)
    {
        item?.Use(character, this);
    }
}