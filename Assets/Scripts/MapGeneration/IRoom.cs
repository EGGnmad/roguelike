using UnityEngine;

namespace MapGeneration
{
    public interface IRoom
    {
        public int Index { get; }
        public Vector2 GetSize();
    }
}