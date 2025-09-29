using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.Settings;
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

        public ResetProgressState(IGameStateMachine stateMachine, IHexagonFactory hexagonFactory,
            IGameFactory gameFactory, IGameProgressService progressService)
        {
            _stateMachine = stateMachine;
            _hexagonFactory = hexagonFactory;
            _gameFactory = gameFactory;
            _progressService = progressService;
        }

        public void Enter(bool newProgress)
        {
            foreach (GridCell gridCell in _gameFactory.GridCells)
                gridCell.RemoveStack();

            foreach (HexagonStack stack in _hexagonFactory.Stacks)
                if (stack != null)
                    Object.Destroy(stack.gameObject);

            _hexagonFactory.SettingsReaders.Clear();
            _hexagonFactory.Stacks.Clear();

            if (newProgress)
            {
                _progressService.InitializeNewProgress();

                _progressService.SavePersistentProgressAsync();
                _progressService.SaveControlPointProgressAsync();
            }
            else
                _progressService.PersistentProgressToControlPoint();

            _progressService.ApplyProgress();

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
        }
    }
}