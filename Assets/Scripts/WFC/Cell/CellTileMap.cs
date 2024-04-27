using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/CellTileMap")]
    public class CellTileMap : ScriptableObject
    {
        [TableList] public CellTile[] cellTiles;
    }
}