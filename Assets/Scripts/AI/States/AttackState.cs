namespace AI
{
    public class AttackState : StateBehaviorBase<AiState>
    {
        public override AiState GetStateKey() => AiState.Attack;

        public override void StateEnter()
        {
        }

        public override void StateUpdate()
        {
        }

        public override void StateExit()
        {
        }
    }
}