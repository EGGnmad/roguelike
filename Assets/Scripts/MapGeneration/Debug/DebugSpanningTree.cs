using UnityEngine;

namespace MapGeneration.Debug
{
    public class DebugSpanningTree : MonoBehaviour
    {
        private ISpanningTree _spanningTree;
        
        [SerializeField] private float _vertexRadius;
        [SerializeField] private Color _vertexColor;
        [SerializeField] private Color _edgeColor;

        private void Start()
        {
            _spanningTree = GetComponent<ISpanningTree>();
        }

        private void OnDrawGizmos()
        {
            if (_spanningTree == null || _spanningTree.GetSpanningTree() == null) return;

            foreach (var edge in _spanningTree.GetSpanningTree())
            {
                Gizmos.color = _vertexColor;
                Gizmos.DrawLine(edge.point0.position, edge.point1.position);

                Gizmos.color = _edgeColor;
                Gizmos.DrawSphere(edge.point0.position, _vertexRadius);
                Gizmos.DrawSphere(edge.point1.position, _vertexRadius);
            }
        }
    }
}