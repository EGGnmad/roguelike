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
            if (_triangulation == null || _triangulation.GetTriangulation() == null) return;
        
            foreach (var triangle in _triangulation.GetTriangulation().triangles)
            {
                Gizmos.color = _vertexColor;
                Gizmos.DrawSphere(triangle.vertex0.position, _vertexRadius);
                Gizmos.DrawSphere(triangle.vertex1.position, _vertexRadius);
                Gizmos.DrawSphere(triangle.vertex2.position, _vertexRadius);
                
                Gizmos.color = _edgeColor;
                Gizmos.DrawLine(triangle.edge0.point0.position, triangle.edge0.point1.position);
                Gizmos.DrawLine(triangle.edge1.point0.position, triangle.edge1.point1.position);
                Gizmos.DrawLine(triangle.edge2.point0.position, triangle.edge2.point1.position);
            }
        }
        
#endif
    }
}