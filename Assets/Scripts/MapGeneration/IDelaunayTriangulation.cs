using DelaunayTriangulation;

namespace MapGeneration
{
    public interface IDelaunayTriangulation
    {
        public Triangulation GetTriangulation();
    }
}