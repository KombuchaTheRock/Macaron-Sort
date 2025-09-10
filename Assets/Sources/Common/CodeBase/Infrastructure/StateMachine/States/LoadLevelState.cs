using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _factory;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
        }
        
        public void Enter(string levelName)
        {
            _sceneLoader.Load(levelName, OnLoaded);
        }

        private void OnLoaded()
        {
            _factory.CreateInstanceRoot();
            GridGenerator gridGenerator = _factory.CreateGridGenerator(GridTemplate.Default, Vector3.zero);
            gridGenerator.GenerateGrid();
            
            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {
            
        }
    }
}