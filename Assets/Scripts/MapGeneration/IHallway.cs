using DelaunayTriangulation;

namespace MapGeneration
{
    public interface IHallway
    {
        public Edge[] GetHallwayEdges();
    }
}