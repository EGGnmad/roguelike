using UnityEngine;

namespace MapGeneration
{
    public interface IRoom
    {
        public int Index { get; set; }
        public Vector2 GetSize();
    }
}