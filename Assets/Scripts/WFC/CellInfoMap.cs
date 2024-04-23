using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/Cell Map")]
    public class CellInfoMap : ScriptableObject
    {
        [TableList, HideDuplicateReferenceBox] public CellInfoBase[] cells;
    }
}