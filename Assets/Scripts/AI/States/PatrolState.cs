using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    public class PatrolState : StateBehaviorBase<AiState>
    {
        private AiController _controller;

        private Vector2 _origin;
        private Vector2 _dest;

        private void Start()
        {
            _controller = GetComponent<IStateMachine<AiState>>() as AiController;
            _origin = transform.position;
        }

        private void SetDestination()
        {
            _dest = _origin + Random.insideUnitCircle * 3f;
        }

        #region Methods:FSM

        public override AiState GetStateKey() => AiState.Patrol;
        
        public override void StateEnter()
        {
            SetDestination();
        }

        public override void StateUpdate()
        {
            // 초기화
            ContextMap cm = _controller.cm;
            cm.Clear();
            
            var colliders = Physics2D.OverlapCircleAll(transform.position, _controller.detectRadius, LayerMask.GetMask("Character"));

            foreach (var obj in colliders)
            {
                if (obj.gameObject == gameObject) continue;

                if (obj.CompareTag("Player"))
                {
                    // 플레이어를 찾으면 쫓기
                    _controller.ChangeState(AiState.Chase);
                }

                else if (obj.CompareTag("Enemy"))
                {
                    if (Vector2.Distance(transform.position, obj.transform.position) > 1f) continue;
                    cm.Add(obj.transform.position - transform.position, ContextMap.Mode.Danger);
                }
            }
            
            cm.Add(_dest - (Vector2)transform.position);
            if (Vector2.Distance(transform.position, _dest) <= 0.5f)
            {
                SetDestination();
            }
            
            _controller.character.Move(cm.GetDestination());
            cm.Clear();
        }

        public override void StateExit()
        {
            _controller?.cm.Clear();
        }
        
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            
            if (Application.isPlaying)
            {
                Gizmos.DrawWireSphere(_origin, 3f);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position, 3f);
            }
        }
#endif
    }
}