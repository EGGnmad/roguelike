using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/Cell Map")]
    public class CellMap : ScriptableObject
    {
        [TableList, HideDuplicateReferenceBox] public CellBase[] cells;
    }
}