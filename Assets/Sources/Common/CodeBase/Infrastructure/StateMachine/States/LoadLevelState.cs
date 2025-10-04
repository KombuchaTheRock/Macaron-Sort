using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IGameFactory _factory;
        private readonly IUIFactory _uiFactory;
        private readonly IGridGenerator _gridGenerator;
        private readonly IStaticDataService _staticData;
        private readonly IGameProgressService _progressService;
        private readonly IGameSettings _gameSettings;
        private readonly IStackMerger _stackMerger;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory, IUIFactory uiFactory, IGridGenerator gridGenerator, IStaticDataService staticData,
            IGameProgressService progressService, IGameSettings gameSettings, IStackMerger stackMerger)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _factory = factory;
            _uiFactory = uiFactory;
            _gridGenerator = gridGenerator;
            _staticData = staticData;
            _progressService = progressService;
            _gameSettings = gameSettings;
            _stackMerger = stackMerger;
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
            _factory.CreateHUD();
            _uiFactory.CreateUIRoot();
            
            HexagonGrid hexagonGrid = GenerateGrid();
            
            _stackMerger.Initialize(hexagonGrid);
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