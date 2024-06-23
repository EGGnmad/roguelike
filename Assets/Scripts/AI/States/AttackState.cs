using System;
using Cysharp.Threading.Tasks;
using Game.Character;
using UnityEngine;

namespace AI
{
    public class AttackState : StateBehaviorBase<AiState>
    {
        [SerializeField] private AttackItem _attackItem;
        private AiController _controller;

        private void Start()
        {
            _controller = GetComponent<IStateMachine<AiState>>() as AiController;
        }
        
        public override AiState GetStateKey() => AiState.Attack;

        public override async void StateEnter()
        {
            // 공격
            _attackItem.Use(_controller.character as Character, null);
            
            // 0.5초 기다리기
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            
            // 다시 플레이어 쫓기
            _controller.ChangeState(AiState.Chase);
        }

        public override void StateUpdate()
        {
        }

        public override void StateExit()
        {
        }
    }
}