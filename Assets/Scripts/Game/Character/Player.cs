using Game.Character;
using MapGeneration;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = System.Random;

[SelectionBase, RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    #region Fields:Serialized

    [TabGroup("Inventory"), HideLabel] public Inventory inventory;

    #endregion

    #region Methods:Unity

    private void Awake()
    {
        inventory = new Inventory(15, this);
    }

    #endregion

    #region Methods:PlayerStart

    public void PlayerStart(GlobalMap globalMap)
    {
        var randomRoom = globalMap.Rooms[new Random().Next(globalMap.Rooms.Count)];
        transform.position = randomRoom.transform.position;
    }

    #endregion

    public override void Died()
    {
        base.Died();
        //TODO: Die Scene
    }
}
