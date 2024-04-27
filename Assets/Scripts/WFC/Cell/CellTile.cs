using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    [CreateAssetMenu(menuName ="WFC/CellTile")]
    public class CellTile : TileBase
    {
        // 타일, 소켓 인덱스를 가짐
        #region Fields:Serialized

        [PreviewField(64, ObjectFieldAlignment.Center), HideLabel] public Sprite sprite;
        [HideInTables] public int socketUp;
        [HideInTables] public int socketDown;
        [HideInTables] public int socketLeft;
        [HideInTables] public int socketRight;

        #endregion

        public int this[Vector2Int pos]
        {
            get
            {
                if (pos == Vector2Int.up) return socketUp;
                else if (pos == Vector2Int.down) return socketDown;
                else if (pos == Vector2Int.left) return socketLeft;
                else if (pos == Vector2Int.right) return socketRight;
                return -1;
            }
        }
        
        // Unity Tile과 관련된 기능들
        #region Methods:Tile

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (!sprite) return;
            tileData.sprite = Instantiate(sprite);
        }

        #endregion
    }
}
