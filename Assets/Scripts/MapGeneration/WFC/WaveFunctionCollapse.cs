using System.Collections.Generic;
using System.Linq;
using MapGeneration.Abstract;
using UnityEngine;
using Random = System.Random;

namespace MapGeneration
{
    public class WaveFunctionCollapse
    {
        #region Fields:Private

        protected Random _random;
        protected int _width, _height;
        protected CellSuperposition[,] _cells;

        #endregion
        
        #region Fields:Abstract
        
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

        public WaveFunctionCollapse(CellSuperposition[,] cells, Random random)
        {
            _random = random;
            _width = cells.GetLength(0);
            _height = cells.GetLength(1);
            
            _cells = cells;
        }

        #endregion

        #region Methods:WFC

        protected virtual Vector2Int GetMinEntropyPos()
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

        protected virtual IReadOnlyList<Vector2Int> GetPossibleDirections(Vector2Int pos)
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
                    
                    if(!otherCellSuper.Possibilities.Any()) continue;

                    List<int> currentSockets = _cells[current.x, current.y].Possibilities.Select(x => x[direction]).ToList();
                    
                    foreach (var cell in otherCellSuper.Possibilities.ToList())
                    {
                        int otherSocket = cell[-direction];
                        
                        if (!currentSockets.Contains(otherSocket))
                        {
                            otherCellSuper.RemovePossibility(cell);
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
                    cellTiles[i, j] = _cells[i, j].Possibilities.First();
                }
            }

            return cellTiles;
        }
        
        #endregion
    }
}