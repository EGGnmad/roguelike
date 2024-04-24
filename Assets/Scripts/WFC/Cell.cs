using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using Random = System.Random;

namespace MapGeneration
{
    // 하나의 타일을 배치할 Cell(1x1)
    public class Cell
    {
        private Vector2 _pos; // 타일을 채울 위치
        private IEnumerable<CellInfoBase> _possibility; // 가능성(양자역학)
        private bool _isCollapsed; // 붕괴했는가?
        private CellInfoBase _cell; // 모든 가능성이 붕괴되어 관측된 하나의 타일
        private Random _random;
        [Inject] private GlobalMap _map;

        public int Entropy
        {
            get
            {
                if (_isCollapsed) return 1;
                return _possibility.Count();
            }
        }
        public CellInfoBase CellInfo => _cell;
        public Vector2 Pos => _pos;
        
        //Hard coding
        public Cell upCell;
        public Cell downCell;
        public Cell leftCell;
        public Cell rightCell;

        #region Methods:Constructors

        public Cell(Vector2 pos, [NotNull] IEnumerable<CellInfoBase> possibility)
        {
            _pos = pos;
            _possibility = possibility;
            _isCollapsed = false;
            _random = new Random();
        }
        public Cell(Vector2 pos, [NotNull] IEnumerable<CellInfoBase> possibility, Random random)
        {
            _pos = pos;
            _possibility = possibility;
            _isCollapsed = false;
            _random = random; // 랜덤 시드를 위해서
        }

        #endregion
        
        public void Collapse()
        {
            if (_isCollapsed) return;
            
            _isCollapsed = true;
            _cell = _possibility.ElementAt(_random.Next(_possibility.Count()));
        }

        public void ReducePossibility(IEnumerable<CellInfoBase> possibility)
        {
            if (_isCollapsed) return;
            
            _possibility = _possibility.Where(x => possibility.FirstOrDefault(p => p.Equals(x)));
        }

        public void Fill()
        {
            if (!_isCollapsed) return;
            
            Vector3Int pos = new Vector3Int((int)_pos.x, (int)_pos.y);
            _map.floorTilemap.SetTile(pos, _cell.GetTile());
        }
    }
}