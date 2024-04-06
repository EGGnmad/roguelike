using UnityEngine;

namespace MapGeneration.Debug
{
    public class DebugDelaunayTriangulation : MonoBehaviour
    {
        private IDelaunayTriangulation _triangulation;

        [SerializeField] private float _vertexRadius;
        [SerializeField] private Color _vertexColor;
        [SerializeField] private Color _edgeColor;

#if UNITY_EDITOR
        
        private void Start()
        {
            _triangulation = GetComponent<IDelaunayTriangulation>();
        }

        private void OnDrawGizmos()
        {
            //Draw Delaunay triangle
            if (_triangulation == null || _triangulation.GetEdges() == null) return;
        
            foreach (var edge in _triangulation.GetEdges())
            {
                Gizmos.color = _vertexColor;
                Gizmos.DrawSphere(edge.point0.position, _vertexRadius);
                Gizmos.DrawSphere(edge.point1.position, _vertexRadius);
                
                Gizmos.color = _edgeColor;
                Gizmos.DrawLine(edge.point0.position, edge.point1.position);
            }
        }
        
#endif
    }
}