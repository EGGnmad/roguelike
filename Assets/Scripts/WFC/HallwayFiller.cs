using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using Random = System.Random;

namespace MapGeneration
{
    [RequireComponent(typeof(IRoom))]
    public class HallwayFiller : MonoBehaviour, IGenerator
    {
        [SerializeField] private CellTileList loadCellTiles; 
        
        [Inject] private GlobalMap _globalMap;
        private IRoom _hallway;
        
        [Button]
        public void Generate()
        {
            _hallway = GetComponent<IRoom>();
            
            // 채울 타일맵 지정
            _globalMap.SetLayer(GlobalMapLayer.Hallway);

            // 기본 변수
            Random random = new Random();
            Vector2 pos = (Vector2)transform.position - _hallway.GetSize() / 2f;
            
            // 랜덤하게 타일 채우기 (WFC X)
            for (int i = 0; i < _hallway.GetSize().x; i++)
            {
                for (int j = 0; j < _hallway.GetSize().y; j++)
                {
                    Vector3Int posInt = new Vector3Int(i + (int)pos.x, j + (int)pos.y);

                    CellTile randomCellTile = loadCellTiles.cellTiles[random.Next(loadCellTiles.cellTiles.Length)];
                    _globalMap.CurrentTilemap.SetTile(posInt, randomCellTile);
                }
            }
        }
    }
}