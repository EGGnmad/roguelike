using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/Cell")]
    public class CellInfo : CellInfoBase
    {
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellInfoBase[] _upCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellInfoBase[] _downCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellInfoBase[] _leftCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellInfoBase[] _rightCells;

        public override CellInfoBase[] GetUpCells() => _upCells;
        public override CellInfoBase[] GetDownCells() => _downCells;
        public override CellInfoBase[] GetLeftCells() => _leftCells;
        public override CellInfoBase[] GetRightCells() => _rightCells;
    }
}