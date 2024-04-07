using UnityEngine;

namespace MapGeneration
{
    public class HallwayBehavior : MonoBehaviour, IRoom
    {
        public int Index => -1;
        public Vector2 GetSize() => transform.localScale;
    }
}