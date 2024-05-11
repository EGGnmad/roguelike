using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/CellTileMap")]
    public class CellTileList : ScriptableObject
    {
        [TableList] public CellTile[] cellTiles;
    }
}