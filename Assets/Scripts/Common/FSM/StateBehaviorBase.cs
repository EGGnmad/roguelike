using System;
using UnityEngine;

public abstract class StateBehaviorBase<T> : MonoBehaviour, IState where T : Enum
{
    public abstract T GetStateKey();
    
    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
}