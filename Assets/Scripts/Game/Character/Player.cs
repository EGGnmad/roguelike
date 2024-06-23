using System;
using Cysharp.Threading.Tasks;
using Game.Character;
using MapGeneration;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = System.Random;

[SelectionBase, RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    #region Fields:Serialized

    [TabGroup("Inventory"), HideLabel] public Inventory inventory;
    [TabGroup("Events"), SerializeField] private UnityEvent _playerDied;

    #endregion

    public override Vector2 LookDirection => (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

    #region Methods:Unity

    private void Awake()
    {
        var newInventory = new Inventory(15, this);
        if (inventory != null)
        {
            newInventory.Clone(inventory);
        }
        inventory = newInventory;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            inventory.UseItem();
        }
    }

    #endregion

    #region Methods:PlayerStart

    public void PlayerStart(GlobalMap globalMap)
    {
        var randomRoom = globalMap.Hallways[new Random().Next(globalMap.Hallways.Count)];
        transform.position = randomRoom.transform.position;
    }

    #endregion

    public override async void Died()
    {
        _playerDied.Invoke();
        _controller.StopControl();

        await UniTask.Delay(TimeSpan.FromSeconds(3f));

        SceneManager.LoadScene("LobbyScene");
    }
}
