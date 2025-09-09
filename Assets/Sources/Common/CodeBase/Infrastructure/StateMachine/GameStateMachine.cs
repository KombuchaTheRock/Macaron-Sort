using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine, IInitializable
    {
        private Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;
        private GameStateFactory _gameStateFactory;

        public GameStateMachine(GameStateFactory gameStateFactory)
        {
            _gameStateFactory = gameStateFactory;
        }
        
        public void Initialize()
        {
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = _gameStateFactory.CreateState<BootstrapState>(),
                [typeof(LoadProgressState)] = _gameStateFactory.CreateState<LoadProgressState>(),
                [typeof(LoadLevelState)] = _gameStateFactory.CreateState<LoadLevelState>(),
                [typeof(GameLoopState)] = _gameStateFactory.CreateState<GameLoopState>(),
            };

            Enter<BootstrapState>();
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
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

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}