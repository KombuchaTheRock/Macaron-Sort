using System.Linq;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class ResetProgressState : IPayloadedState<bool>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IHexagonFactory _hexagonFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IGameProgressService _progressService;
        private readonly IBoosterActivator _boosterActivator;

        public ResetProgressState(IGameStateMachine stateMachine, IHexagonFactory hexagonFactory,
            IGameFactory gameFactory, IGameProgressService progressService, IBoosterActivator boosterActivator)
        {
            _stateMachine = stateMachine;
            _hexagonFactory = hexagonFactory;
            _gameFactory = gameFactory;
            _progressService = progressService;
            _boosterActivator = boosterActivator;
        }

        public void Enter(bool newProgress)
        {
            _boosterActivator.Reset();
            
            ClearStacks();
            LoadOrInitializeNewProgress(newProgress);

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }

        private void LoadOrInitializeNewProgress(bool newProgress)
        {
            if (newProgress)
            {
                _progressService.InitializeNewProgress();

                _progressService.SavePersistentProgressAsync();
                _progressService.SaveControlPointProgressAsync();
            }
            else
                _progressService.PersistentProgressToControlPoint();

            _progressService.ApplyProgress();
        }

        private void ClearStacks()
        {
            foreach (GridCell gridCell in _gameFactory.GridCells)
                gridCell.RemoveStack();

            foreach (HexagonStack stack in _hexagonFactory.Stacks.Where(stack => stack != null))
                Object.Destroy(stack.gameObject);

            _hexagonFactory.SettingsReaders.Clear();
            _hexagonFactory.Stacks.Clear();
        }
    }
}