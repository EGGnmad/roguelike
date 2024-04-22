using Sirenix.OdinInspector;
using UnityEngine;

namespace MapGeneration
{
    [CreateAssetMenu(menuName = "WFC/Cell")]
    public class Cell : CellBase
    {
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellBase[] _upCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellBase[] _downCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellBase[] _leftCells;
        [TableList, HideDuplicateReferenceBox, HideInTables, SerializeField] private CellBase[] _rightCells;

        public override CellBase[] GetUpCells() => _upCells;
        public override CellBase[] GetDownCells() => _downCells;
        public override CellBase[] GetLeftCells() => _leftCells;
        public override CellBase[] GetRightCells() => _rightCells;
    }
}