using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _factory;
        private string _currentLevelName;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
        }

        public void Enter(string levelName)
        {
            _currentLevelName = levelName;
            _sceneLoader.Load(levelName, OnLoaded);
        }

        private void OnLoaded()
        {
            InitializeGameWorld();

            _stateMachine.Enter<GameLoopState>();
        }

        private void InitializeGameWorld()
        {
            _factory.CreateInstanceRoot();

            GridGenerator gridGenerator = _factory.CreateGridGenerator(GridTemplate.Default, Vector3.zero);
            gridGenerator.GenerateGrid();

            StackGenerator stackGenerator =
                _factory.CreateStackGenerator(HexagonStackTemplate.Default, _currentLevelName, Vector3.zero);
            stackGenerator.GenerateStacks();
        }

        public void Exit()
        {
        }
    }
}