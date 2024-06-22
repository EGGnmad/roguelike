using Game.Character;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/AttackItem")]
public class AttackItem : Item
{
    public OwnedGameObject attackPrefab;
    
    public override void Use(Character character, ItemSlot slot)
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - character.transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        OwnedGameObject attackObj = GameObject.Instantiate(attackPrefab, character.transform.position + (Vector3)dir, Quaternion.Euler(0, 0, angle));
        attackObj.SetOwner(character);
    }
}