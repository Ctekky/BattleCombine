using System;
using System.Collections.Generic;
using BattleCombine.Interfaces;

namespace BattleCombine.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootStrapState)] =
                    new BootStrapState(this, sceneLoader),
                [typeof(MenuState)] =
                    new MenuState(this, sceneLoader),
                [typeof(LoadLevelState)] =
                    new LoadLevelState(this, sceneLoader),
                [typeof(GameLoopState)] =
                    new GameLoopState(this),
                [typeof(PauseState)] =
                    new PauseState(this),
            };
        }

        public void Enter<TState>() where TState : class, IGameState
        {
            IGameState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>()
            where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}