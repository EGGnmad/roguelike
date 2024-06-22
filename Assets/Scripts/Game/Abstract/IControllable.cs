using UnityEngine;

public interface IControllable
{
    public void ControlStarted(IController controller);
    public void Move(Vector2 dir);
}