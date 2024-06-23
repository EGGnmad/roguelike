using Game.Character;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/HealItem")]
public class HealItem : Item
{
    [SerializeField] private int amount;
    
    public override void Use(Character character, ItemSlot slot)
    {
        character.stats.Hp += amount;
        
        Player player = character as Player;
        player.inventory.RemoveItem(slot);
    }
}
