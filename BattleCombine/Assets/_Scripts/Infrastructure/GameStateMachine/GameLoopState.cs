using BattleCombine.Interfaces;

namespace BattleCombine.Infrastructure
{
    public class GameLoopState : IGameState
    {
        private readonly GameStateMachine _stateMachine;

        public GameLoopState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Exit()
        {
        }

        public void Enter()
        {
        }
    }
}