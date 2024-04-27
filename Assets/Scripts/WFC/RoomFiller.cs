using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = System.Random;

namespace MapGeneration
{
    [RequireComponent(typeof(IRoom))]
    public class RoomFiller : MonoBehaviour, IGenerator
    {
        #region Fields:Serialized

        [InlineButton("SetSeedToRandom", SdfIconType.Dice6Fill, "")] public int seed;
        [SerializeField] private GlobalMapLayer _layer;
        [SerializeField] private CellTileMap _cellTileMap;        

        #endregion
        
        #region Fields:Private

        private IRoom _room;
        private WaveFunctionCollapse _wfc;
        [Inject] private GlobalMap _globalMap;

        #endregion

        private void Awake()
        {
            _room = GetComponent<IRoom>();
        }

        public void Fill(CellTile[,] cells)
        {
            _globalMap.SetLayer(_layer);
            
            Vector2 pos = (Vector2)transform.position - _room.GetSize() / 2f;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Vector2Int add = new Vector2Int(i, j);
                    Vector3Int posInt = new Vector3Int((int)pos.x + add.x, (int)pos.y + add.y);
                    
                    _globalMap.CurrentTilemap.SetTile(posInt, cells[i,j]);
                }
            }
        }

        public async void Generate()
        {
            if (_room == null) _room = GetComponent<IRoom>();
            
            int width = (int)_room.GetSize().x;
            int height = (int)_room.GetSize().y;
            _wfc = new WaveFunctionCollapse(width, height, _cellTileMap.cellTiles);
            CellTile[,] cells = await UniTask.RunOnThreadPool(_wfc.Collapse);
            Fill(cells);
        }

        #region Methods:Other

        private void SetSeedToRandom()
        {
            seed = new Random().Next();
        }

        #endregion
    }
}