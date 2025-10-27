using System.Collections.Generic;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.BoosterSystem.Activation;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

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
        private readonly IBoosterActivator _boosterActivator;
        private readonly SceneLoader _sceneLoader;

        public LoadLevelState(IGameStateMachine stateMachine, SceneLoader sceneLoader, IGameFactory factory, IUIFactory uiFactory, IGridGenerator gridGenerator, IStaticDataService staticData,
            IGameProgressService progressService, IGameSettings gameSettings, IStackMerger stackMerger, IBoosterActivator boosterActivator)
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
            _boosterActivator = boosterActivator;
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
            _uiFactory.CreateUIRoot();
            
            GameObject hud = _factory.CreateHUD();
            BoosterPicker boosterPicker = hud.GetComponentInChildren<BoosterPicker>();
            HexagonGrid hexagonGrid = GenerateGrid();
            
            _progressService.ApplyProgress();
            
            boosterPicker.Initialize(hexagonGrid); 
            _boosterActivator.Initialize(boosterPicker, hexagonGrid);
            _stackMerger.Initialize(hexagonGrid);
            
            CameraUtility.UpdateCameraSize(hexagonGrid);
            
            _gameSettings.ApplySettings();
        }

        private HexagonGrid GenerateGrid()
        {
            GridConfig gridConfig = _staticData.GameConfig.GridConfig;

            List<CellData> cellData = _progressService.GameProgress.PersistentProgressData.WorldData.GridData.Cells;
            bool savedGridExists = cellData.Count > 0;
            
            HexagonGrid hexagonGrid = savedGridExists ? _gridGenerator.GenerateSavedGrid(gridConfig.CellConfig, cellData) : 
                _gridGenerator.GenerateNewGrid(gridConfig.Size, gridConfig.CellConfig);
            
            return hexagonGrid;
        }
    }
}