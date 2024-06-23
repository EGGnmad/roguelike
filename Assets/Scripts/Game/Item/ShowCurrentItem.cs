using System;
using UnityEngine;
using VContainer;


[RequireComponent(typeof(SpriteRenderer))]
public class ShowCurrentItem : MonoBehaviour
{
    public float distance;
    
    private Player _player;
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        //
        if (_player.inventory.Index >= _player.inventory.Size) return;
        
        ItemSlot slot = _player.inventory[_player.inventory.Index];
        if (slot == null)
        {
            _sr.sprite = null;
            return;
        }
        
        Item item = slot.item;
        //>> 에러 핸들링

        // 플레이어 -> 마우스 방향
        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _player.transform.position;
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        
        // 위치
        float mag = Mathf.Clamp(dir.magnitude, 0.2f, distance);
        Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * mag;
        
        angle = angle % 180f - 90f;

        // 반영
        transform.position = _player.transform.position + (Vector3)offset;
        transform.localEulerAngles = new Vector3(0, 0, angle);
        _sr.sprite = item.icon;
    }
}