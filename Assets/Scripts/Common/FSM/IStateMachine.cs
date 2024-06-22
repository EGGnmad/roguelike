using System;

public interface IStateMachine<T> where T : Enum
{
    public T GetCurrentStateKey();
    public void ChangeState(T newStateKey);
}