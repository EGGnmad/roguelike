using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    // 원래 인터페이스는 유니티 에디터 상에서 보이지 않지만 ShowInInspector를 사용하면 가능하다. (for Test)
    [ShowInInspector] private IControllable _character;
    
    public void ControlStart(IControllable character)
    {
        _character = character;
        _character.ControlStarted(this);
    }

    public void StopControl()
    {
        _character?.Move(Vector2.zero);
        _character = null;
    }

    private void Start()
    {
        IControllable player = GameObject.FindWithTag("Player")?.GetComponent<IControllable>();
        ControlStart(player);
    }

    public void Update()
    {
        if (_character == null) return;
        
        // 키보드 인풋
        Vector2 inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _character.Move(inputAxis.normalized);
    }
}