using UnityEditor;
using UnityEngine;

namespace MapGeneration.Debug
{
    [RequireComponent(typeof(IRoom)), ExecuteInEditMode]
    public class DebugRoom : MonoBehaviour
    {
        private IRoom _room;
        
        [SerializeField] Color roomColor = Color.blue;
        [SerializeField] Color textColor = Color.blue;

#if UNITY_EDITOR
        
        private void Start()
        {
            _room = GetComponent<IRoom>();
        }
        
        private void OnDrawGizmos()
        {
            if (_room == null) return;
            
            var position = transform.position;
            
            Gizmos.color = roomColor;
            Gizmos.DrawWireCube(position, _room.GetSize());

            GUIStyle style = new GUIStyle();
            style.normal.textColor = textColor;

            Vector2 offset = _room.GetSize() / 2f;
            offset.x *= -1f;

            Vector2 pos = (Vector2)position + offset;
            Handles.Label(pos, _room.Index.ToString(), style);
        }
        
#endif
    }
}