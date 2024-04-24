using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public abstract class CellInfoBase : ScriptableObject
    {
        public int id;
        [PreviewField(height:64, ObjectFieldAlignment.Center), HideLabel, SerializeField] 
        private TileBase _tile;
        
        public virtual TileBase GetTile() => _tile;
        public abstract CellInfoBase[] GetUpCells();
        public abstract CellInfoBase[] GetDownCells();
        public abstract CellInfoBase[] GetLeftCells();
        public abstract CellInfoBase[] GetRightCells();
    }
}