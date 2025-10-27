using System.Linq;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.BoosterSystem.Activation;
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
        private SceneLoader _sceneLoader;
        private IPauseService _pauseService;

        public ResetProgressState(IGameStateMachine stateMachine, IHexagonFactory hexagonFactory,
            IGameFactory gameFactory, IGameProgressService progressService, IBoosterActivator boosterActivator,
            SceneLoader sceneLoader, IPauseService pauseService)
        {
            _pauseService = pauseService;
            _sceneLoader = sceneLoader;
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

            _sceneLoader.Load(SceneNames.Bootstrap,
                () =>
                {
                    _stateMachine.Enter<LoadLevelState, string>(SceneNames.Gameplay);
                    _pauseService.Unpause();
                });
        }

        public void Exit()
        {
        }

        private void LoadOrInitializeNewProgress(bool newProgress)
        {
            if (newProgress)
                _progressService.InitializeNewProgress();
            else
                _progressService.PersistentProgressToControlPoint();

            _progressService.SavePersistentProgressAsync();
            _progressService.SaveControlPointProgressAsync();
         }

        private void ClearStacks()
        {
            foreach (GridCell gridCell in _gameFactory.GridCells)
                gridCell.FreeCell();

            foreach (HexagonStack stack in _hexagonFactory.Stacks.Where(stack => stack != null))
                Object.Destroy(stack.gameObject);

            _gameFactory.ProgressReaders.Clear();
            _hexagonFactory.SettingsReaders.Clear();
            _hexagonFactory.Stacks.Clear();
        }
    }
}