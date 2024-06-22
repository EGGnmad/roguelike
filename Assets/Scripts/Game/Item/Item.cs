using System.Collections;
using Game.Character;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class Item : ScriptableObject, IComparer
{
    [PreviewField]
    public Sprite icon;
    public new string name;
    public int stackSize = 1;
    
    [Multiline]
    public string description;

    public virtual void Use(Character character, ItemSlot slot)
    {
        Debug.Log($"{character.name} used item {name}");
    }

    public int Compare(object x, object y)
    {
        Item itemX = x as Item;
        Item itemY = y as Item;
        
        if (itemX == null || itemY == null) return -1;
        if (itemX.name != itemY.name) return -1;

        return 0;
    }
}