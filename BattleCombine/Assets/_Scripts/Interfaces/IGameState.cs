namespace BattleCombine.Interfaces
{
    public interface IGameState : IExitableState
    {
        void Enter();
    }

    public interface IExitableState
    {
        void Exit();
    }

    public interface IPayloadedState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}