using System;
using System.Collections.Generic;
using DelaunayTriangulation;

public class MinimumSpanningTree : ISpanningTree
{
    public float randomPathValue;
    private Edge[] _edges;
    private DisjointSet<int> _disjoint;
    
    public MinimumSpanningTree(Edge[] edges, float pathValue = 0f)
    {
        _edges = edges;
        _disjoint = new DisjointSet<int>();

        randomPathValue = pathValue;
    }
    
    public Edge[] GetSpanningTree()
    {
        Array.Sort(_edges);

        List<Edge> results = new();

        for (int i = 0; i < _edges.Length; i++)
        {
            int a = _edges[i].point0.index;
            int b = _edges[i].point1.index;

            if (_disjoint.Find(a) == _disjoint.Find(b))
            {
                if (UnityEngine.Random.value < randomPathValue)
                {
                    results.Add(_edges[i]);
                }
                continue;
            }
            
            _disjoint.Union(a, b);
            results.Add(_edges[i]);
        }

        return results.ToArray();
    }
}