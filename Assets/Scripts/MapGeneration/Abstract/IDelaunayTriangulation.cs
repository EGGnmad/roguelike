using DelaunayTriangulation;

namespace MapGeneration
{
    public interface IDelaunayTriangulation
    {
        public Edge[] GetEdges();
    }
}