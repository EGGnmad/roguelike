using Game.Character;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/AttackItem")]
public class AttackItem : Item
{
    public OwnedGameObject attackPrefab;
    
    public override void Use(Character character, ItemSlot slot)
    {
        Vector2 dir = character.LookDirection;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        OwnedGameObject attackObj = GameObject.Instantiate(attackPrefab, character.transform.position + (Vector3)offset, Quaternion.Euler(0, 0, angle));
        attackObj.SetOwner(character);
    }
}