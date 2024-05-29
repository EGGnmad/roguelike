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

        public void SetLayer(GlobalMapLayer layer)
        {
            _layer = layer;
        }
    }
}