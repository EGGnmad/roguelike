using System.Collections.Generic;
using System.Linq;
using MapGeneration.Abstract;
using UnityEngine;
using Random = System.Random;

namespace MapGeneration
{
    public class WaveFunctionCollapse : IWaveFunctionCollapse, ISuperposition
    {
        #region Fields:Private

        private Random _random;
        private int _width, _height;
        private CellSuperposition[,] _cells;

        #endregion
        
        #region Fields:Abstract

        public CellSuperposition[,] Cells => _cells;

        public bool IsCollapsed
        {
            get
            {
                bool isCollapsed = true;

                foreach (var cellSuperposition in _cells)
                {
                    isCollapsed = isCollapsed && cellSuperposition.IsCollapsed;
                }

                return isCollapsed;
            }
        }

        #endregion

        #region Methods:Ctor

        public WaveFunctionCollapse(int width, int height, IEnumerable<CellTile> allCells, int seed = 0)
        {
            _random = new Random(seed);
            _width = width;
            _height = height;
            
            _cells = new CellSuperposition[width, height];
            
            var defaultCells = allCells as CellTile[] ?? allCells.ToArray();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _cells[i, j] = new CellSuperposition(defaultCells, _random);
                }
            }
        }


        #endregion

        #region Methods:WFC

        protected Vector2Int GetMinEntropyPos()
        {
            Vector2Int pos = Vector2Int.zero;
            int minEntropy = -1;
            
            for (int i = 0; i < _cells.GetLength(0); i++)
            {
                for (int j = 0; j < _cells.GetLength(1); j++)
                {
                    if (minEntropy == -1 && !_cells[i, j].IsCollapsed)
                    {
                        pos = new Vector2Int(i, j);
                        minEntropy = _cells[i, j].Entropy;
                    }
                    
                    else if (!_cells[i, j].IsCollapsed && minEntropy > _cells[i, j].Entropy)
                    {
                        pos = new Vector2Int(i, j);
                        minEntropy = _cells[i, j].Entropy;
                    }
                }
            }

            return pos;
        }

        protected IReadOnlyList<Vector2Int> GetPossibleDirections(Vector2Int pos)
        {
            List<Vector2Int> result = new();
            
            if(pos.y + 1 < _height) result.Add(Vector2Int.up);
            if(pos.y - 1 >= 0) result.Add(Vector2Int.down);
            if(pos.x + 1 < _width) result.Add(Vector2Int.right);
            if(pos.x - 1 >= 0) result.Add(Vector2Int.left);

            return result;
        }

        private void Propagate(Vector2Int index)
        {
            Stack<Vector2Int> stack = new();
            stack.Push(index);

            while (stack.Count > 0)
            {
                Vector2Int current = stack.Pop();

                foreach (var direction in GetPossibleDirections(current))
                {
                    Vector2Int newPos = current + direction;
                    CellSuperposition otherCellSuper = _cells[newPos.x, newPos.y];
                    
                    if(otherCellSuper.cells.Count == 0) continue;

                    List<int> currentSockets = _cells[current.x, current.y].cells.Select(x => x[direction]).ToList();
                    
                    foreach (var cell in otherCellSuper.cells.ToList())
                    {
                        int otherSocket = cell[-direction];
                        
                        if (!currentSockets.Contains(otherSocket))
                        {
                            otherCellSuper.cells.Remove(cell);
                            if (!stack.Contains(newPos))
                            {
                                stack.Push(newPos);
                            }
                        }
                    }
                }
            }
        }
        
        public CellTile[,] Collapse()
        {
            while (!IsCollapsed)
            {
                Vector2Int pos = GetMinEntropyPos();
                var cell = _cells[pos.x, pos.y];
                cell.Collapse();
                Propagate(pos);
            }

            CellTile[,] cellTiles = new CellTile[_width, _height];
            for (int i = 0; i < _cells.GetLength(0); i++)
            {
                for (int j = 0; j < _cells.GetLength(1); j++)
                {
                    cellTiles[i, j] = _cells[i, j].cells[0];
                }
            }

            return cellTiles;
        }

        #endregion
    }
}