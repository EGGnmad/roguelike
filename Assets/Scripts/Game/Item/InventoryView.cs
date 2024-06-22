using System;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _index = 0;

    #region Fields:Player
    
    [Inject] private Player _player;
    private Inventory _inventory;

    #endregion

    #region Fields:Event

    public UnityEvent<Inventory, int> InventoryViewChanged;
    
    #endregion
    
    #region Fields:KeyInput

    private Timestamped<long> _prevLastKeyInput;
    private Timestamped<long> _nextLastKeyInput;
    
    private IDisposable _prevKeyInputStream;
    private IDisposable _nextKeyInputStream;
    private IDisposable _useKetInputStream;
    
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
                if((x.Timestamp - _prevLastKeyInput.Timestamp).TotalSeconds > 0.6f)
                {
                    _prevLastKeyInput = x;
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
                if((x.Timestamp - _nextLastKeyInput.Timestamp).TotalSeconds > 0.6f)
                {
                    _nextLastKeyInput = x;
                    return true;
                }
                return false;
            })
            .Subscribe(x =>
            {
                Next();
            });
        
        // 아이템 사용
        _useKetInputStream = keyInputStream.Where(_ => Input.GetMouseButtonDown(0)).Subscribe(x => UseItem());
    }

    private void OnDestroy()
    {
        _prevKeyInputStream.Dispose();
        _nextKeyInputStream.Dispose();
        _useKetInputStream.Dispose();
    }

    #endregion

    #region Methods:Inventory

    public void Next()
    {
        if (_index >= _inventory.Size - 1) return;

        _animator.Play("InventorySwapLeft");
        _index = Mathf.Clamp(_index + 1, 0, _inventory.Size - 1);
        OnInventoryViewChanged();
    }

    public void Prev()
    {
        if (_index <= 0) return;
        
        _animator.Play("InventorySwapRight");
        _index = Mathf.Clamp(_index - 1, 0, _inventory.Size - 1);
        OnInventoryViewChanged();
    }

    public void UseItem()
    {
        ItemSlot slot = _inventory[_index];
        _inventory.UseItem(slot);
    }

    private void OnInventoryViewChanged()
    {
        InventoryViewChanged?.Invoke(_inventory, _index);
    }

    #endregion
}
