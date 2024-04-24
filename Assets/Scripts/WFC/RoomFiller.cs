using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;
using VContainer;
using Random = Unity.Mathematics.Random;

namespace MapGeneration
{
    [RequireComponent(typeof(IRoom))]
    public class RoomFiller : MonoBehaviour, IGenerator
    {
        [InlineButton("SetRandomSeed", SdfIconType.Dice6Fill, "")] public int seed;
        [AssetsOnly, SerializeField] private CellInfoMap _cellInfoMap;
        [SerializeField] private GlobalMapLayer _fillLayer;

        [Inject] private GlobalMap _globalMap;
        private IRoom _room;
        private int _width, _height;
        private NativeList<int>[] _cells;
        private Dictionary<int, CellInfoBase> _cellInfo;
        private Random _random;
        
        #region Methods:Generation

        private void Awake()
        {
            _room = GetComponent<IRoom>();
        }

        [BurstCompile]
        public void Init()
        {
            var defaultCells = new NativeArray<int>(_cellInfoMap.cells.Length, Allocator.Persistent);
            for (int i = 0; i < _cellInfoMap.cells.Length; i++)
            {
                defaultCells[i] = _cellInfoMap.cells[i].id;
            }
                
            _cells = new NativeList<int>[_width * _height];
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new NativeList<int>(Allocator.Persistent);
                _cells[i].CopyFrom(defaultCells);
            }

            // End
            defaultCells.Dispose();
        }

        [BurstCompile]
        private uint GetMinEntropyIndex()
        {
            if (_cells.Length == 0) throw new Exception("Zero Area Exception");
            
            int minEntropy = _cells[0].Length;
            uint index = 0;

            for (uint i = 0; i < _cells.Length; i++)
            {
                if ((minEntropy == 1) || (minEntropy > _cells[i].Length && _cells[i].Length > 1))
                {
                    minEntropy = _cells[i].Length;
                    index = i;
                }
            }

            return index;
        }

        [BurstCompile]
        private void CollapseCell(uint index)
        {
            int randIndex = _random.NextInt(_cells[index].Length);
            int temp = _cells[index][randIndex];
            _cells[index].Clear();
            _cells[index].Add(temp);
        }
        
        private void ReducePossibilities(uint index, IEnumerable<int> newPossibility)
        {
            NativeList<int> cell = _cells[index];
            var possibility = newPossibility as int[] ?? newPossibility.ToArray();
            NativeHashMap<int, bool> compress = new NativeHashMap<int, bool>(possibility.Count(), Allocator.TempJob);

            for (int i = 0; i < possibility.Count(); i++)
            {
                int current = possibility.ElementAt(i);
                compress.Add(current, true);
            }

            for (int i = 0; i < cell.Length; i++)
            {
                int current = cell[i];
                if (!compress.ContainsKey(current))
                {
                    cell.RemoveAt(i);
                }
            }
            
            //End
            compress.Dispose();
        }

        public void Collapse()
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                uint minIndex = GetMinEntropyIndex();
                int x = (int)(minIndex / _height);
                int y = (int)(minIndex % _height);
                
                CollapseCell(minIndex);

                int currentCell = _cells[minIndex][0];
                var cell = _cellInfo[currentCell];
                
                // up
                if(y+1 < _height) ReducePossibilities((uint)(x*_height+y+1), cell.GetUpCells().Select(c => c.id)); 
                if(y-1 >= 0) ReducePossibilities((uint)(x*_height+y-1), cell.GetDownCells().Select(c => c.id)); 
                if(x+1 < _width) ReducePossibilities((uint)((x+1)*_height+y), cell.GetRightCells().Select(c => c.id)); 
                if(x-1 >= 0) ReducePossibilities((uint)((x-1)*_height+y), cell.GetLeftCells().Select(c => c.id)); 
            }
        }

        public void Fill()
        {
            _globalMap.SetLayer(_fillLayer);
            
            for (int i = 0; i < _cells.Length; i++)
            {
                int x = (i / _height);
                int y = (i % _height);
                
                int currentCell = _cells[i][0];
                var cell = _cellInfo[currentCell];

                Vector2 pf = (Vector2)transform.position + new Vector2(x, y) - _room.GetSize() / 2;
                Vector3Int pos = new Vector3Int((int)pf.x, (int)pf.y);
                _globalMap.CurrentTilemap.SetTile(pos, cell.GetTile());
            }
        }

        [Button]
        public async void Generate()
        {
            _random = new Random((uint)seed);
            _width = (int)_room.GetSize().x;
            _height = (int)_room.GetSize().y;
            _cellInfo = new Dictionary<int, CellInfoBase>();
            foreach (var cell in _cellInfoMap.cells)
            {
                _cellInfo.Add(cell.id, cell);
            }
            
            await UniTask.RunOnThreadPool(() =>
            {
                Init();
                Collapse();
            });
            
            Fill();
            
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i].Dispose();
            }
        }

        #endregion

        #region Methods:Others

        public void SetRandomSeed()
        {
            seed = new System.Random().Next();
        }

        #endregion
    }
}