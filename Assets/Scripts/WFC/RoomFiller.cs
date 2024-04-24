using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = System.Random;

namespace MapGeneration
{
    [RequireComponent(typeof(IRoom))]
    public class RoomFiller : MonoBehaviour, IGenerator
    {
        [InlineButton("SetRandomSeed", SdfIconType.Dice6Fill, "")] 
        public int seed;
        [AssetsOnly, SerializeField] 
        private CellInfoMap _cellInfoMap;
        
        private IRoom _room;
        private Cell[] _cells;
        private Random _random;
        [Inject] private IObjectResolver _container;
        
        #region Methods:Generation

        private void Awake()
        {
            _room = GetComponent<IRoom>();
            _random = new Random(seed);
        }

        public void Init()
        {
            int w = (int)_room.GetSize().x;
            int h = (int)_room.GetSize().y;
            
            _cells = new Cell[w*h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Vector2 pos = transform.position + new Vector3(i, j) - (Vector3)_room.GetSize() / 2f;
                    _cells[i*h + j] = new Cell(pos, _cellInfoMap.cells);
                    _container.Inject(_cells[i*h + j]);
                }
            }
            
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int x, y;
                    
                    // up cell
                    x = i + 1;
                    y = j;
                    if (x < w) _cells[i * h + j].upCell = _cells[x * h + y];
                    
                    // down cell
                    x = i - 1;
                    y = j;
                    if (x >= 0) _cells[i * h + j].upCell = _cells[x * h + y];
                    
                    // left cell
                    x = i;
                    y = j - 1;
                    if (y >= 0) _cells[i * h + j].upCell = _cells[x * h + y];
                    
                    // right cell
                    x = i;
                    y = j + 1;
                    if (y < h) _cells[i * h + j].upCell = _cells[x * h + y];
                }
            }
        }

        public void Collapse()
        {
            var sorted = _cells.OrderBy(x => x.Entropy);

            for (int i = 0; i < _cells.Length; i++)
            {
                Cell cell = sorted.First(x => x.Entropy > 1);

                cell.Collapse();
                cell.upCell?.ReducePossibility(cell.CellInfo.GetUpCells());
                cell.downCell?.ReducePossibility(cell.CellInfo.GetDownCells());
                cell.leftCell?.ReducePossibility(cell.CellInfo.GetLeftCells());
                cell.rightCell?.ReducePossibility(cell.CellInfo.GetRightCells());
            }
        }

        public void Fill()
        {
            foreach (var cell in _cells)
            {
                cell.Fill();
            }
        }

        [Button("Generate")]
        public void Generate()
        {
            Init();
            Collapse();
            Fill();
        }

        #endregion

        public void SetRandomSeed()
        {
            seed = new Random().Next();
        }
    }
}