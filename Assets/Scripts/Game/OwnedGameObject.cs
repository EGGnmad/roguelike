using Game.Character;
using UnityEngine;

public class OwnedGameObject : MonoBehaviour
{
    protected Character _owner;
    
    public virtual void SetOwner(Character character)
    {
        _owner = character;
    }
}