using System;
using System.Collections.Generic;
using MapGeneration.Abstract;

namespace MapGeneration
{
    public class CellSuperposition : ISuperposition
    {
        private List<CellTile> _cells;
        private Random _random = new();

        #region Fields:Abstract

        public bool IsCollapsed { get; private set; } = false;
        public int Entropy => _cells.Count;
        public IEnumerable<CellTile> Possibilities => _cells;
        
        #endregion

        #region Methods:Ctor

        public CellSuperposition(IEnumerable<CellTile> defaultCells)
        {
            _cells = new List<CellTile>(defaultCells);
        }

        public CellSuperposition(IEnumerable<CellTile> defaultCells, Random random) : this(defaultCells)
        {
            _random = random;
        }

        public CellSuperposition(CellTile defaultCells)
        {
            IsCollapsed = true;
            _cells = new();
            _cells.Add(defaultCells);
        }

        #endregion

        public void Collapse()
        {
            if (IsCollapsed) return;
            
            IsCollapsed = true;
            CellTile temp = _cells[_random.Next(_cells.Count)];
            _cells.Clear();
            _cells.Add(temp);
        }

        public void RemovePossibility(CellTile possibility)
        {
            _cells.Remove(possibility);
        }
    }
}