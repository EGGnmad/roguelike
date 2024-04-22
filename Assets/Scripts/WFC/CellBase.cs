using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public abstract class CellBase : ScriptableObject
    {
        [PreviewField(height:64, ObjectFieldAlignment.Center), HideLabel, SerializeField] private TileBase _tile;
        
        public virtual TileBase GetTile() => _tile;
        public abstract CellBase[] GetUpCells();
        public abstract CellBase[] GetDownCells();
        public abstract CellBase[] GetLeftCells();
        public abstract CellBase[] GetRightCells();
    }
}