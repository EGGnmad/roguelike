using System;
using System.Collections.Generic;
using DelaunayTriangulation;

public class MinimumSpanningTree : ISpanningTree
{
    public float randomPathValue;
    private Edge[] _edges;
    private Dictionary<int, int> _parent;
    
    public MinimumSpanningTree(Edge[] edges, float pathValue = 0f)
    {
        _edges = edges;
        _parent = new Dictionary<int, int>();

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

            if (Find(a) == Find(b))
            {
                if (UnityEngine.Random.value < randomPathValue)
                {
                    results.Add(_edges[i]);
                }
                continue;
            }
            
            Union(a, b);
            results.Add(_edges[i]);
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