using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 원래 인터페이스는 유니티 에디터 상에서 보이지 않지만 ShowInInspector를 사용하면 가능하다. (for Test)
    [ShowInInspector] private IControllable _character;
    
    public void ControlStart(IControllable character)
    {
        _character = character;
    }

    public void Update()
    {
        if (_character == null) return;
        
        // 키보드 인풋
        Vector2 inputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        _character.Move(inputAxis.normalized);
    }
}