using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridModificator : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private HexagonGrid _hexagonGrid;
        [SerializeField] private GridCellUnlocker _gridCellUnlocker;
        [SerializeField, Min(0)] private int _cellsToAddCount;
        [SerializeField, Min(0)] private int _cellsToDeleteCount;

        private GridCellAddLogic _gridCellsAddLogic;
        private GridCellDeleteLogic _gridCellDeleteLogic;
        private GridCellLockLogic _gridCellsLockLogic;
        private Dictionary<int, GridModifier> _gridModifiers;

        private IGridGenerator _gridGenerator;
        private IStaticDataService _staticData;
        private IPlayerLevel _playerLevel;
        private IStackMerger _stackMerger;

        public event Action GridModified;

        [Inject]
        private void Construct(IStaticDataService staticData, IGridGenerator gridGenerator, IPlayerLevel playerLevel, IStackMerger stackMerger)
        {
            _stackMerger = stackMerger;
            _playerLevel = playerLevel;
            _staticData = staticData;
            _gridGenerator = gridGenerator;
        }

        private void Awake()
        {
            _gridModifiers =
                _staticData.GameConfig.GridModifierConfig.Modifiers
                    .ToDictionary(x => x.LevelForStart, x => x);

            _gridCellsAddLogic = new GridCellAddLogic(_gridGenerator, _staticData, this, _hexagonGrid);
            _gridCellDeleteLogic = new GridCellDeleteLogic(_hexagonGrid, this);
            _gridCellsLockLogic = new GridCellLockLogic(_hexagonGrid, this);

            SubscribeUpdates();
        }

        private void OnDestroy() =>
            CleanUp();

        private void SubscribeUpdates() =>
            _playerLevel.LevelChanged += OnLevelChanged;

        private void CleanUp() =>
            _playerLevel.LevelChanged -= OnLevelChanged;

        private void OnLevelChanged(int level)
        {
            // if (_gridModifiers.TryGetValue(level, out GridModifier modifier) == false)
            //     return;
            
            ChooseAndApplyModifier();
        }

        private void ChooseAndApplyModifier()
        {
            int freeCellsCount = _hexagonGrid.Cells.Count(x => x is { IsOccupied: false, IsLocked: false });
            float freeCellsPercentage = freeCellsCount / (float)_hexagonGrid.Cells.Count;

            Debug.Log($"FreeCellsCount: {freeCellsCount}\nFreeCellsPercentage: {freeCellsPercentage}");
            
            switch (freeCellsPercentage)
            {
                case <= 0.40f:
                    ApplyCellModifier(AddCells, freeCellsCount, 0.15f, 0.25f);
                    break;
                case > 0.75f:
                    ApplyCellModifier(RemoveCells, freeCellsCount, 0.15f, 0.25f);                    
                    break;
                case > 0.65f:
                    ApplyCellModifier(LockCellsByTileScore, freeCellsCount, 0.10f, 0.15f);
                    break;
                case > 0.55f:
                    ApplyCellModifier(LockCellsSimple, freeCellsCount, 0.10f, 0.15f);
                    break;
            }
        }

        private void ApplyCellModifier(Action<int> modifier, int freeCellsCount, float minPercentage, float maxPercentage)
        {
            int count = GetCellCountInRange(freeCellsCount, minPercentage, maxPercentage);
            Debug.Log($"Modifying {count} of {freeCellsCount} cells");
            modifier(count);
        }

        private int GetCellCountInRange(int cellsCount, float minPercent, float maxPercent)
        {
            int minCount = Mathf.CeilToInt(cellsCount * minPercent);
            int maxCount = Mathf.CeilToInt(cellsCount * maxPercent);

            int count = Random.Range(minCount, maxCount);
            return count;
        }

        [Button("AddCells")]
        private void AddCells(int count = 1)
        {
            _gridCellsAddLogic.AddCellsToRandomPositions(count,
                () => GridModified?.Invoke());
        }

        [Button("RemoveCells")]
        private void RemoveCells(int count = 1)
        {
            _gridCellDeleteLogic.DeleteRandomEdgeFreeCells(count,
                () => GridModified?.Invoke());
        }
        
        [Button("BlockCellSimple")]
        private void LockCellsSimple(int count = 1)
        {
            _gridCellsLockLogic.AddSimpleLocks(count, simpleCellLocks =>
            {
                if (simpleCellLocks.Count <= 0)
                    return;

                _gridCellUnlocker.AddSimpleCellLocks(simpleCellLocks);
                GridModified?.Invoke();
            });
        }

        [Button("BlockTileScoreCellLock")]
        private void LockCellsByTileScore(int count = 1)
        {
            _gridCellsLockLogic.AddTileScoreLocks(count, tileScoreCellLocks =>
            {
                if (tileScoreCellLocks.Count <= 0)
                    return;

                _gridCellUnlocker.AddTileScoreCellLocks(tileScoreCellLocks);
                GridModified?.Invoke();
            });
        }
    }
}