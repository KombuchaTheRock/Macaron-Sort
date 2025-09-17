using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _factory;
        private readonly IStackGenerator _stackGenerator;
        private readonly IGridGenerator _gridGenerator;
        private readonly IStaticDataService _staticData;
        private readonly SceneLoader _sceneLoader;
        
        private string _currentLevelName;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory,
            IStackGenerator stackGenerator, IGridGenerator gridGenerator, IStaticDataService staticData)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
            _stackGenerator = stackGenerator;
            _gridGenerator = gridGenerator;
            _staticData = staticData;
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

            GenerateGrid();
            GenerateStacks();
        }

        private void GenerateGrid()
        {
            GridConfig gridConfig = _staticData.GameConfig.GridConfig;
            _gridGenerator.GenerateGrid(gridConfig.Grid, gridConfig.Size, gridConfig.CellConfig);
        }

        private void GenerateStacks()
        {
            HexagonStackConfig stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
            Vector3[] stackSpawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();
            
            _stackGenerator.GenerateStacks(stackSpawnPositions,
                stackConfig.MinStackSize,
                stackConfig.MaxStackSize,
                stackConfig.HexagonHeight,
                stackConfig.Colors);
        }

        public void Exit()
        {
        }
    }
}