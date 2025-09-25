using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private const float DelayBetweenStacks = 0.2f;

        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;
        private readonly IPlayerLevel _playerLevel;
        private readonly IGameProgressService _gameProgressService;
        private readonly IStackGenerator _stackGenerator;

        private StackMover _stackMover;
        private MergeSystem _mergeSystem;
        private List<HexagonStack> _generatedStacks;
        private HexagonStackConfig _stackConfig;

        public GameLoopState(IGameFactory factory, IStackGenerator stackGenerator,
            IStaticDataService staticData, IPlayerLevel playerLevel, IGameProgressService gameProgressService)
        {
            _stackGenerator = stackGenerator;
            _factory = factory;
            _staticData = staticData;
            _playerLevel = playerLevel;
            _gameProgressService = gameProgressService;
        }

        public void Enter()
        {
            _generatedStacks = new List<HexagonStack>();
            _stackMover = _factory.StackMover;
            _mergeSystem = _factory.MergeSystem;
            _stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
            
            _mergeSystem.UpdateOccupiedCells();
            
            SubscribeUpdates();
            
            GenerateStacks(_stackConfig);
        }

        public void Exit() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _stackMover.StackPlaced += OnStackPlaced;
            _stackMover.DragStarted += OnDragStarted;
            _stackMover.DragFinished += OnDragFinished;

            _mergeSystem.MergeStarted += OnMergeStarted;
            _mergeSystem.MergeFinished += OnMergeFinished;
            _mergeSystem.StackCompleted += OnStackCompleted;

            _playerLevel.ControlPointAchieved += OnControlPointAchieved;
        }

        private void OnControlPointAchieved() => 
            SaveControlPointData(_generatedStacks);

        private void CleanUp()
        {
            _stackMover.StackPlaced -= OnStackPlaced;
            _stackMover.DragStarted -= OnDragStarted;
            _stackMover.DragFinished -= OnDragFinished;

            _mergeSystem.MergeStarted -= OnMergeStarted;
            _mergeSystem.MergeFinished -= OnMergeFinished;
        }

        private void OnStackCompleted(int score) =>
            _playerLevel.AddScore(score);

        private void OnMergeStarted() =>
            UpdateGridRotationEnabled();

        private void OnMergeFinished() =>
            UpdateGridRotationEnabled();

        private void OnDragFinished() =>
            UpdateGridRotationEnabled();

        private void OnDragStarted() =>
            UpdateGridRotationEnabled();

        private void OnStackPlaced(GridCell cell)
        {
            _generatedStacks.Remove(cell.Stack);

            UpdateFreeStacksData(_generatedStacks);

            if (_generatedStacks.Count <= 0)
            {
                Vector3[] stackSpawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();
                GenerateNewStacks(stackSpawnPositions, _stackConfig, OnStacksGenerated);
            }
        }

        private void UpdateGridRotationEnabled()
        {
            if (_stackMover.IsDragging || _mergeSystem.IsMerging)
                _factory.GridRotator.enabled = false;
            else
                _factory.GridRotator.enabled = true;
        }

        private void GenerateStacks(HexagonStackConfig stackConfig)
        {
            List<FreeStack> freeStacks = _gameProgressService.GameProgress.PersistentProgressData.WorldData.StacksData
                .FreeStacks;

            if (freeStacks.Count <= 0)
            {
                Vector3[] stackSpawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();
                GenerateNewStacks(stackSpawnPositions, stackConfig, OnStacksGenerated);
            }
            else
            {
                GenerateSavedStacks(freeStacks, stackConfig, OnStacksGenerated);
            }
        }

        private void GenerateNewStacks(Vector3[] stackSpawnPositions, HexagonStackConfig stackConfig,
            Action<List<HexagonStack>> onStacksGenerated)
        {
            _stackGenerator.GenerateStacks(stackSpawnPositions,
                stackConfig,
                DelayBetweenStacks,
                onStacksGenerated);
        }

        private void GenerateSavedStacks(List<FreeStack> freeStacks, HexagonStackConfig stackConfig,
            Action<List<HexagonStack>> onStacksGenerated)
        {
            List<HexagonStack> generatedStacks = new();

            foreach (FreeStack freeStack in freeStacks)
            {
                HexagonStack hexagonStack =
                    _stackGenerator.GenerateStack(freeStack.SpawnPosition, stackConfig, freeStack.Tiles);

                generatedStacks.Add(hexagonStack);
            }

            onStacksGenerated(generatedStacks);
        }

        private void OnStacksGenerated(List<HexagonStack> stacks)
        {
            _generatedStacks = stacks;

            UpdateFreeStacksData(_generatedStacks);
        }

        private void UpdateFreeStacksData(List<HexagonStack> stacks)
        {
            StacksData stacksData = _gameProgressService.GameProgress.PersistentProgressData.WorldData.StacksData;
            stacksData.UpdateFreeStacksData(stacks);
        }

        private void SaveControlPointData(List<HexagonStack> stacks)
        {
            StacksData stacksData = _gameProgressService.GameProgress.ControlPointProgressData.WorldData.StacksData;
            stacksData.UpdateFreeStacksData(stacks);
            
            _gameProgressService.SaveControlPointProgressAsync();
        }
    }
}