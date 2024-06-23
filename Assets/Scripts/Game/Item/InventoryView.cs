using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    #region Fields:Player
    
    [Inject] private Player _player;
    private Inventory _inventory;

    #endregion

    #region Fields:Event

    public UnityEvent<Inventory> InventoryViewChanged;
    
    #endregion
    
    #region Fields:KeyInput

    private float minInputTime = 0.6f;
    private Timestamped<long> _lastKeyInput;
    
    private IDisposable _prevKeyInputStream;
    private IDisposable _nextKeyInputStream;
    
    #endregion

    #region Methods:Unity

    private void Start()
    {
        _inventory = _player.inventory;
        _inventory.PropertyChanged += (sender, args) =>
        {
            OnInventoryViewChanged();

        };
        OnInventoryViewChanged();

        
        // 업데이트 스트림
        var keyInputStream = Observable.EveryUpdate();
        
        // 이전 아이템 인풋
        _prevKeyInputStream = keyInputStream.Where(_ => Input.GetKey(KeyCode.Z))
            .Timestamp()
            .Where(x =>
            {
                if((x.Timestamp - _lastKeyInput.Timestamp).TotalSeconds >= minInputTime)
                {
                    _lastKeyInput = x;
                    return true;
                }
                return false;
            })
            .Subscribe(x =>
            {
                Prev();
            });
        
        // 이후 아이템 인풋
        _nextKeyInputStream = keyInputStream.Where(_ => Input.GetKey(KeyCode.X))
            .Timestamp()
            .Where(x =>
            {
                if((x.Timestamp - _lastKeyInput.Timestamp).TotalSeconds >= minInputTime)
                {
                    _lastKeyInput = x;
                    return true;
                }
                return false;
            })
            .Subscribe(x =>
            {
                Next();
            });
    }

    private void OnDestroy()
    {
        _prevKeyInputStream.Dispose();
        _nextKeyInputStream.Dispose();
    }

    #endregion

    #region Methods:Inventory

    public void Next()
    {
        if (_inventory.Index >= _inventory.Size - 1) return;

        _animator.Play("InventorySwapLeft");
        _inventory.Next();
        OnInventoryViewChanged();
    }

    public void Prev()
    {
        if (_inventory.Index <= 0) return;
        
        _animator.Play("InventorySwapRight");
        _inventory.Prev();
        OnInventoryViewChanged();
    }

    private void OnInventoryViewChanged()
    {
        InventoryViewChanged?.Invoke(_inventory);
    }

    #endregion
}
