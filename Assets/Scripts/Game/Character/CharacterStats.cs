using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public struct CharacterStats
{
    [ProgressBar(0, 200, r: 1, g: 0, b: 0), LabelText(SdfIconType.HeartFill), ShowInInspector]
    public int maxHp;
    
    private int _hp;
    public int Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            _hp = Mathf.Clamp(_hp, 0, maxHp);
        }
    }
    
    [ProgressBar(0, 100), LabelText(SdfIconType.Magic), ShowInInspector]
    public int damage;

    [ProgressBar(0, 10, r: 0, g: 1, b: 0), LabelText(SdfIconType.LightningFill), ShowInInspector]
    public float speed;

    public CharacterStats(int maxHp=0, int damage=0, int speed=0)
    {
        this.maxHp = maxHp;
        _hp = maxHp;
        this.damage = damage;
        this.speed = speed;
    }
}