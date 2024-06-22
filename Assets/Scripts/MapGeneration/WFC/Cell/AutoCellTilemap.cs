using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public class AutoCellTilemap : MonoBehaviour, IGenerator
    {
        [SerializeField] private Tilemap _inputTilemap;
        
        private DisjointSet<int> _disjoint;
        private Dictionary<CellTile, int> _cellTileIndex;

        private Dictionary<CellTile, int> GetCellTileIndex()
        {
            Dictionary<CellTile, int> index = new();
            
            BoundsInt bounds = _inputTilemap.cellBounds;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    Vector3Int pos = bounds.position + new Vector3Int(i, j);
                    var cell = _inputTilemap.GetTile<CellTile>(pos);
                    
                    if(!cell || index.ContainsKey(cell)) continue;
                    index.Add(cell, index.Count * 4);
                }
            }

            return index;
        }

        private void SetCellTileSocket(Vector2Int pos)
        {
            var cell = _inputTilemap.GetTile<CellTile>((Vector3Int)pos);
            if (!cell) return;
            
            var up = _inputTilemap.GetTile<CellTile>((Vector3Int)(pos + Vector2Int.up));
            if (up)
            {
                int cI = _cellTileIndex[cell];
                int i = _cellTileIndex[up]+2;
                _disjoint.Union(cI, i);
            }
            
            var right = _inputTilemap.GetTile<CellTile>((Vector3Int)(pos + Vector2Int.right));
            if (right)
            {
                int cI = _cellTileIndex[cell]+1;
                int i = _cellTileIndex[right]+3;
                _disjoint.Union(cI, i);
            }

            var down = _inputTilemap.GetTile<CellTile>((Vector3Int)(pos + Vector2Int.down));
            if (down)
            {
                int cI = _cellTileIndex[cell]+2;
                int i = _cellTileIndex[down];
                _disjoint.Union(cI, i);
            }
            
            var left = _inputTilemap.GetTile<CellTile>((Vector3Int)(pos + Vector2Int.left));
            if (left)
            {
                int cI = _cellTileIndex[cell]+3;
                int i = _cellTileIndex[left]+1;
                _disjoint.Union(cI, i);
            }
        }
        
        [Button]
        public void Generate()
        {
            _disjoint = new DisjointSet<int>();
            
            _cellTileIndex = GetCellTileIndex();
            
            BoundsInt bounds = _inputTilemap.cellBounds;
            for (int i = 0; i < bounds.size.x; i++)
            {
                for (int j = 0; j < bounds.size.y; j++)
                {
                    Vector3Int pos = bounds.position + new Vector3Int(i, j);
                    SetCellTileSocket((Vector2Int)pos);
                }
            }

            foreach (var key in _cellTileIndex.Keys)
            {
                // up : 0
                int index = _cellTileIndex[key];
                if (_disjoint.GetRank(index) == 0) key.socketUp = -1;
                else key.socketUp = _disjoint.Find(index);
                
                // right : 1
                index = _cellTileIndex[key] + 1;
                if (_disjoint.GetRank(index) == 0) key.socketRight = -1;
                else key.socketRight = _disjoint.Find(index);
                
                // down : 2
                index = _cellTileIndex[key] + 2;
                if (_disjoint.GetRank(index) == 0) key.socketDown = -1;
                else key.socketDown = _disjoint.Find(index);
                
                // left : 3
                index = _cellTileIndex[key] + 3;
                if (_disjoint.GetRank(index) == 0) key.socketLeft = -1;
                else key.socketLeft = _disjoint.Find(index);
            }
        }
    }
}