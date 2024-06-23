using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGeneration
{
    public enum GlobalMapLayer
    {
        Floor,
        Collider
    }
    
    public class GlobalMap : SerializedMonoBehaviour
    {
        public Tilemap this[GlobalMapLayer layer] => _tilemaps[layer]; 
        public Tilemap CurrentTilemap => _tilemaps[_layer];
        
        [SerializeField] private Dictionary<GlobalMapLayer, Tilemap> _tilemaps;
        private GlobalMapLayer _layer;
        
        // Maps
        private RoomBehavior[] _rooms;
        public IReadOnlyList<RoomBehavior> Rooms => _rooms;
        
        private HallwayBehavior[] _hallways;
        public IReadOnlyList<HallwayBehavior> Hallways => _hallways;

        public void SetLayer(GlobalMapLayer layer)
        {
            _layer = layer;
        }

        public void SetRooms(RoomBehavior[] rooms)
        {
            _rooms = rooms;
        }

        public void SetHallways(HallwayBehavior[] hallways)
        {
            _hallways = hallways;
        }
    }
}