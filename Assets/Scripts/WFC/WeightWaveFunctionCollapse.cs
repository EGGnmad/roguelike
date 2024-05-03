using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration
{
    public class WeightWaveFunctionCollapse : WaveFunctionCollapse
    {
        public WeightWaveFunctionCollapse(int width, int height, IEnumerable<CellTile> allCells, int seed = 0) : base(width, height, allCells, seed)
        {
        }

        protected override Vector2Int GetMinEntropyPos()
        {
            return base.GetMinEntropyPos();
        }
    }
}