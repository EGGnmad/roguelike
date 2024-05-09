using System;
using System.Collections.Generic;

public class DisjointSet<T> where T : IComparable<T>
{
    private Dictionary<T, T> _parent;
    private Dictionary<T, int> _rank;

    public DisjointSet()
    {
        _parent = new Dictionary<T, T>();
        _rank = new Dictionary<T, int>();
    }

    public void Union(T a, T b)
    {
        a = Find(a);
        b = Find(b);

        if (_rank[a] >= _rank[b])
        {
            _parent[b] = a;
            _rank[a]++;
        }
        else
        {
            _parent[a] = b;
            _rank[b]++;
        }
    }

    public T Find(T a)
    {
        if (!_parent.ContainsKey(a))
        {
            _parent.Add(a, a);
            _rank.Add(a, 0);
        }
        
        if (_parent[a].CompareTo(a) == 0) return a;
        return _parent[a] = Find(_parent[a]);
    }
}