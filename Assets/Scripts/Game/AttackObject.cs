using System;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AttackObject : OwnedGameObject
{
    public bool usePlayerDamage = false;
    public bool hasOwnDamage = false;
    [EnableIf("hasOwnDamage")] public int damageOffset;
    
    
    public bool destoryOnAttack;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 사용자라면 넘어가기
        if (other.gameObject == _owner.gameObject) return;
        
        IDamagable character = other.GetComponent<IDamagable>();
        if (character == null) return;
        
        // 공격
        int damage = 0;
        if (hasOwnDamage)
        {
            damage += damageOffset;
        }
        if (usePlayerDamage)
        {
            damage += _owner.stats.damage;
        }
        character.GetDamage(damage);
        
        // 공격 후 파괴 여부
        if (destoryOnAttack)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}