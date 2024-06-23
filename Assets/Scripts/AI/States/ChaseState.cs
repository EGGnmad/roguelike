using UnityEngine;

namespace AI
{
    public class ChaseState : StateBehaviorBase<AiState>
    {
        public float maxAttackRadius = 2f;
        private AiController _controller;

        private void Start()
        {
            _controller = GetComponent<IStateMachine<AiState>>() as AiController;
        }

        public override AiState GetStateKey() => AiState.Chase;
        
        public override void StateEnter()
        {
        }

        public override void StateUpdate()
        {
            ContextMap cm = _controller.cm;
            
            // 캐릭터
            var colliders = Physics2D.OverlapCircleAll(transform.position, _controller.detectRadius, LayerMask.GetMask("Character"));

            bool collidePlayer = false;
            foreach (var obj in colliders)
            {
                if (obj.gameObject == gameObject) continue;

                if (obj.CompareTag("Player"))
                {
                    collidePlayer = true;
                    cm.Add(obj.transform.position - transform.position, ContextMap.Mode.Interest);
                    
                    // 플레이와 거리가 반걍 이내라면 공격
                    if (Vector2.Distance(obj.transform.position, transform.position) <= maxAttackRadius)
                    {
                        _controller.ChangeState(AiState.Attack);
                    }
                }
                
                else if (obj.CompareTag("Enemy"))
                {
                    if (Vector2.Distance(transform.position, obj.transform.position) > 1f) continue;
                    cm.Add(obj.transform.position - transform.position, ContextMap.Mode.Danger);
                }
            }

            // 움직이기
            _controller.character.Move(cm.GetDestination());
            // Debug.DrawRay(transform.position, cm.GetDestination(), Color.red);
            cm.Clear();
            
            // 플레이어가 범위에서 벗어나면 순찰
            if (!collidePlayer)
            {
                _controller.ChangeState(AiState.Patrol);
            }
        }

        public override void StateExit()
        {
            _controller?.cm.Clear();
        }
    }
}