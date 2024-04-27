using System;
using System.Collections.Generic;
using MapGeneration.Abstract;

namespace MapGeneration
{
    public class CellSuperposition : ISuperposition
    {
        public List<CellTile> cells;

        private bool _isCollapsed;
        private Random _random;

        #region Fields:Abstract

        public bool IsCollapsed => _isCollapsed;
        public int Entropy => cells.Count;
        #endregion

        #region Methods:Ctor

        public CellSuperposition(IEnumerable<CellTile> defaultCells)
        {
            _isCollapsed = false;
            cells = new List<CellTile>(defaultCells);
            _random = new Random();
        }

        public CellSuperposition(IEnumerable<CellTile> defaultCells, Random random)
        {
            _isCollapsed = false;
            cells = new List<CellTile>(defaultCells);
            _random = random;
        }

        #endregion

        public void Collapse()
        {
            _isCollapsed = true;
            CellTile temp = cells[_random.Next(cells.Count)];
            cells.Clear();
            cells.Add(temp);
        }
    }
}