using System;
using Sirenix.OdinInspector;

[Serializable]
public struct CharacterStats
{
    [ProgressBar(0, 200, r: 1, g: 0, b: 0), LabelText(SdfIconType.HeartFill), ShowInInspector]
    public int hp;

    [ProgressBar(0, 100), LabelText(SdfIconType.Magic), ShowInInspector]
    public int damage;

    [ProgressBar(0, 10, r: 0, g: 1, b: 0), LabelText(SdfIconType.LightningFill), ShowInInspector]
    public float speed;
}