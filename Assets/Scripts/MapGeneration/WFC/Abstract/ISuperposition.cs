using System.Collections.Generic;

namespace MapGeneration.Abstract
{
    public interface ISuperposition
    {
        public bool IsCollapsed { get; }
        public int Entropy { get; }
        public IEnumerable<CellTile> Possibilities { get; }
    }
}