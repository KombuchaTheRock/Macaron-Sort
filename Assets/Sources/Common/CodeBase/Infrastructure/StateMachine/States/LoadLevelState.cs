using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _factory;
        private readonly IGridGenerator _gridGenerator;
        private readonly IStaticDataService _staticData;
        private readonly IGameProgressService _progressService;
        private readonly IGameSettings _gameSettings;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory, IGridGenerator gridGenerator, IStaticDataService staticData,
            IGameProgressService progressService, IGameSettings gameSettings)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
            _gridGenerator = gridGenerator;
            _staticData = staticData;
            _progressService = progressService;
            _gameSettings = gameSettings;
        }

        public void Enter(string levelName) => 
            _sceneLoader.Load(levelName, OnLoaded);

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            InitializeGameWorld();
            _stateMachine.Enter<GameLoopState>();
        }

        private void InitializeGameWorld()
        {
            _factory.CreateInstanceRoot();

            HexagonGrid hexagonGrid = GenerateGrid();
            StackMover stackMover = _factory.CreateStackMover();

            MergeSystem mergeSystem = _factory.CreateMergeSystem(stackMover, hexagonGrid);
            _factory.CreateHUD();

            HexagonGridSaveLoader hexagonGridSaveLoader = hexagonGrid.GetComponent<HexagonGridSaveLoader>();
            hexagonGridSaveLoader.Initialize(mergeSystem);

            GameFinisher gameFinisher = hexagonGrid.GetComponent<GameFinisher>();
            gameFinisher.Initialize(mergeSystem);
            
            _progressService.ApplyProgress();
            _gameSettings.ApplySettings();
        }

        private HexagonGrid GenerateGrid()
        {
            GridConfig gridConfig = _staticData.GameConfig.GridConfig;
            HexagonGrid hexagonGrid = _gridGenerator.GenerateGrid(gridConfig.Grid, gridConfig.Size, gridConfig.CellConfig);
            
            return hexagonGrid;
        }
    }
}