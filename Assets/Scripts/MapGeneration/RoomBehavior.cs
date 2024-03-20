using DelaunayTriangulation;
using UnityEngine;

namespace MapGeneration
{
    [RequireComponent(typeof(Collider2D))]
    public class RoomBehavior : MonoBehaviour, IRoom
    {
        public int Index { get; set; }
        public Vector2 GetSize() => transform.localScale;

        #region Fields:SteeringBehavior
        
        private Vector3 _velocity;
        private Vector3 _position;

        public Vector3 Velocity => _velocity;
        public bool isSeperationStart = false;
        
        #endregion
        
        #region Delaunay
        
        public static implicit operator Vertex(RoomBehavior room)
        {
            return new Vertex(room.transform.position, room.Index);
        }

        #endregion

        #region SteeringBehavior

        private void Start()
        {
            _position = transform.position;
            
            SeparationBehave();
        }

        private void Update()
        {
            SeparationBehave();
            // transform.position += _velocity * Time.deltaTime;
            _position += _velocity * Time.deltaTime;
            
            // transform.position을 그리드에 맞게 이동
            Vector3 roundedPos = _position;
            roundedPos.x = Mathf.Floor(roundedPos.x);
            roundedPos.y = Mathf.Floor(roundedPos.y);
            transform.position = roundedPos;
        }
        
        private void SeparationBehave()
        {
            // 겹치는 방이 있는지 확인
            Collider2D[] agents = Physics2D.OverlapBoxAll(transform.position, GetSize(), transform.rotation.eulerAngles.z);

            // 겹친 오브젝트와 거리벡터의 평균값을 구하고 반대방향으로 이동
            foreach (var agent in agents)
            {
                _velocity += agent.transform.position - transform.position;
            }
            _velocity /= agents.Length - 1;
            _velocity = _velocity.normalized;
            _velocity *= -1f;
            
            isSeperationStart = true;
        }

        #endregion
    }
}