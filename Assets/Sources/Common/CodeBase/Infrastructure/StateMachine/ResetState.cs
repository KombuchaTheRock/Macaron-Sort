using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine
{
    public class ResetState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IHexagonFactory _hexagonFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IGameProgressService _progressService;

        public ResetState(IGameStateMachine stateMachine, IHexagonFactory hexagonFactory, IGameFactory gameFactory, IGameProgressService progressService)
        {
            _stateMachine = stateMachine;
            _hexagonFactory = hexagonFactory;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }
        
        public void Enter()
        {
            foreach (GridCell gridCell in _gameFactory.GridCells)
                gridCell.RemoveStack();
            
            foreach (HexagonStack stack in _hexagonFactory.Stacks)
                if (stack != null)
                    Object.Destroy(stack.gameObject);
            
            _hexagonFactory.Stacks.Clear();

            _progressService.PersistentProgressToControlPoint();
            _progressService.ApplyProgress();
            
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}