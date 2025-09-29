using System.Collections.Generic;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class HexagonGridSaveLoader : MonoBehaviour, IProgressReader
    {
        [SerializeField] private HexagonGrid _hexagonGrid;

        private IGameProgressService _gameProgress;
        private MergeSystem _mergeSystem;
        private List<GridCell> _cells;
        private IStackGenerator _stackGenerator;
        private IStaticDataService _staticData;
        private IPlayerLevel _playerLevel;

        [Inject]
        private void Construct(IGameProgressService gameProgress, IStackGenerator stackGenerator,
            IStaticDataService staticData, IPlayerLevel playerLevel)
        {
            _playerLevel = playerLevel;
            _staticData = staticData;
            _stackGenerator = stackGenerator;
            _gameProgress = gameProgress;
        }

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            _cells = _hexagonGrid.Cells;

            SubscribeUpdates();
        }

        private void OnDestroy() => 
            CleanUp();

        public void ApplyProgress(GameProgress progress)
        {
            List<PlacedStackData> placedStacks = progress.PersistentProgressData.WorldData.StacksData.StacksOnGrid;

            if (placedStacks.Count <= 0)
                return;

            foreach (PlacedStackData placedStack in placedStacks) 
                GenerateStack(placedStack);
        }

        private void GenerateStack(PlacedStackData placedStack)
        {
            HexagonStackConfig stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
            float offsetAboveGridGrid = _staticData.GameConfig.StackMoverConfig.PlaceOffsetAboveGrid;

            if (_hexagonGrid.TryGetCell(placedStack.PositionOnGrid, out GridCell cell) == false) 
                return;
                
            Vector3 spawnPosition = cell.transform.position + Vector3.up * offsetAboveGridGrid;
            HexagonStack stack =
                _stackGenerator.GenerateStack(spawnPosition, stackConfig, placedStack.Tiles);
                    
            stack.transform.parent = cell.transform;
            stack.DisableMovement();
            cell.SetStack(stack);
        }

        private void SubscribeUpdates()
        {
            foreach (GridCell gridCell in _cells)
                gridCell.StackRemoved += UpdateGridPersistentData;

            _playerLevel.ControlPointAchieved += SaveControlPointData;
            
            _mergeSystem.MergeStarted += UpdateGridPersistentData;
            _mergeSystem.MergeFinished += UpdateGridPersistentData;
        }

        private void CleanUp()
        {
            foreach (GridCell gridCell in _cells)
                gridCell.StackRemoved -= UpdateGridPersistentData;

            _mergeSystem.MergeStarted -= UpdateGridPersistentData;
            _mergeSystem.MergeFinished -= UpdateGridPersistentData;
        }

        private void SaveControlPointData()
        {
            StacksData stacksControlPointData = _gameProgress.GameProgress.ControlPointProgressData.WorldData.StacksData;
            stacksControlPointData.UpdateStacksOnGridData(_cells);

            _gameProgress.SaveControlPointProgressAsync();
        }

        private void UpdateGridPersistentData()
        {
            StacksData stacksPersistentData = _gameProgress.GameProgress.PersistentProgressData.WorldData.StacksData;
            stacksPersistentData.UpdateStacksOnGridData(_cells);
        }
    }
}