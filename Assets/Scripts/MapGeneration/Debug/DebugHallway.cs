using UnityEngine;

namespace MapGeneration.Debug
{
    [RequireComponent(typeof(IHallway))]
    public class DebugHallway : MonoBehaviour
    {
        private IHallway _hallway;
        
        [SerializeField] private Color _edgeColor;
        
#if UNITY_EDITOR
        
        private void Start()
        {
            _hallway = GetComponent<IHallway>();
        }

        private void OnDrawGizmos()
        {
            //Draw Delaunay triangle
            if (_hallway == null || _hallway.GetHallwayEdges() == null) return;
        
            foreach (var edge in _hallway.GetHallwayEdges())
            {
                Gizmos.color = _edgeColor;
                Gizmos.DrawLine(edge.point0.position, edge.point1.position);
            }
        }
        
#endif
    }
}