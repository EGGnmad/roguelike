using System;
using System.Collections.Generic;
using DelaunayTriangulation;

public class MinimumSpanningTree : ISpanningTree
{
    public float randomPathValue;
    private Triangulation _triangulation;
    private Dictionary<int, int> _parent;
    
    public MinimumSpanningTree(Triangulation triangulation, float pathValue = 0f)
    {
        _triangulation = triangulation;
        _parent = new Dictionary<int, int>();

        randomPathValue = pathValue;
    }

    private Edge[] GetAllEdges()
    {
        Edge[] edges = new Edge[_triangulation.triangles.Count * 3];
        for (int i = 0; i < _triangulation.triangles.Count; i++)
        {
            edges[i * 3] = _triangulation.triangles[i].edge0;
            edges[i * 3 + 1] = _triangulation.triangles[i].edge1;
            edges[i * 3 + 2] = _triangulation.triangles[i].edge2;
        }

        return edges;
    }
    
    public Edge[] GetSpanningTree()
    {
        Edge[] edges = GetAllEdges();
        Array.Sort(edges);

        List<Edge> results = new();

        for (int i = 0; i < edges.Length; i++)
        {
            int a = edges[i].point0.index;
            int b = edges[i].point1.index;

            if (Find(a) == Find(b))
            {
                if (UnityEngine.Random.value < randomPathValue)
                {
                    results.Add(edges[i]);
                }
                continue;
            }
            
            Union(a, b);
            results.Add(edges[i]);
        }

        return results.ToArray();
    }

    #region UnionFind

    private int Find(int a)
    {
        if(!_parent.ContainsKey(a)) _parent.Add(a, a);
        
        if (_parent[a] == a) return a;
        return _parent[a] = Find(_parent[a]);
    }
    
    private void Union(int a, int b)
    {
        a = Find(a);
        b = Find(b);

        _parent[b] = a;
    }

    #endregion
}