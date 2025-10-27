using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure.Extensions;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using Sources.Features.HexagonSort.StackCompleter;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellUnlocker : MonoBehaviour, IProgressReader
    {
        public event Action GridModified;

        [SerializeField] private HexagonGrid _hexagonGrid;

        private Dictionary<HexagonTileType, SortedSet<TileScoreCellLock>> _tileScoreCellLocks = new();
        private SortedSet<SimpleCellLock> _simpleLockedCells = new();
        private IStackMerger _stackMerger;
        private IStackCompleter _stackCompleter;
        private IGameProgressService _progressService;

        [Inject]
        private void Construct(IStackMerger stackMerger, IStackCompleter stackCompleter)
        {
            _stackCompleter = stackCompleter;
            _stackMerger = stackMerger;
        }

        private void Awake()
        {
            InitializeTileScoreLocks();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            CleanUp();

        public void ApplyProgress(GameProgress progress) =>
            LoadCellLocks();

        public void AddSimpleCellLocks(List<SimpleCellLock> simpleCellLocks) =>
            _simpleLockedCells.AddRange(simpleCellLocks);

        public void AddTileScoreCellLocks(List<TileScoreCellLock> tileScoreCellLocks)
        {
            foreach (HexagonTileType tileType in EnumExtensions.GetAllValues<HexagonTileType>())
                AddTileScoreCellLocksByTileType(tileScoreCellLocks, tileType);
        }

        private void InitializeTileScoreLocks()
        {
            foreach (HexagonTileType tileType in EnumExtensions.GetAllValues<HexagonTileType>())
                _tileScoreCellLocks.Add(tileType, new SortedSet<TileScoreCellLock>());
        }

        private void OnStackCompleted(HexagonStackScore stackScore) =>
            UpdateCellLocks(stackScore);

        private void UpdateCellLocks(HexagonStackScore stackScore)
        {
            if (_simpleLockedCells.Count > 0)
                UpdateSimpleCellLocks();

            if (_tileScoreCellLocks.Count > 0)
                UpdateTileScoreCellLocks(stackScore);

            GridModified?.Invoke();
        }

        private void UpdateTileScoreCellLocks(HexagonStackScore stackScore)
        {
            while (true)
            {
                if (_tileScoreCellLocks[stackScore.TileType].Count == 0) return;

                TileScoreCellLock tileScoreCellLock = _tileScoreCellLocks[stackScore.TileType].First();

                int remainder = CalculateRemainder(stackScore, tileScoreCellLock);

                tileScoreCellLock.DecreaseScoreToUnlock(stackScore.Score);

                if (tileScoreCellLock.IsLocked == false)
                {
                    _tileScoreCellLocks[stackScore.TileType].Remove(tileScoreCellLock);

                    if (remainder > 0)
                    {
                        stackScore = new HexagonStackScore(stackScore.TileType, remainder);
                        continue;
                    }
                }

                break;
            }
        }

        private static int CalculateRemainder(HexagonStackScore stackScore, TileScoreCellLock tileScoreCellLock)
        {
            return tileScoreCellLock.ScoreToUnlock < stackScore.Score
                ? stackScore.Score - tileScoreCellLock.ScoreToUnlock
                : 0;
        }

        private void UpdateSimpleCellLocks()
        {
            SimpleCellLock simpleLockedCell = _simpleLockedCells
                .First();

            simpleLockedCell.DecreaseCompletedStacksToUnlock();

            if (simpleLockedCell.IsLocked == false)
                _simpleLockedCells.Remove(simpleLockedCell);
        }

        private void AddTileScoreCellLocksByTileType(List<TileScoreCellLock> tileScoreCellLocks,
            HexagonTileType tileType)
        {
            List<TileScoreCellLock> cellLocksByTileType = tileScoreCellLocks
                .Where(x => x.TileType == tileType)
                .ToList();

            if (cellLocksByTileType.Count == 0)
                return;

            _tileScoreCellLocks[tileType].AddRange(cellLocksByTileType);
        }

        private void LoadCellLocks()
        {
            List<GridCell> lockedCells = _hexagonGrid.Cells
                .Where(cell => cell.IsLocked)
                .ToList();

            LoadSimpleCellLocks(lockedCells);
            LoadTileScoreCellLocks(lockedCells);
        }

        private void LoadTileScoreCellLocks(List<GridCell> lockedCells)
        {
            List<TileScoreCellLock> tileScoreCellLocks = lockedCells
                .Where(x => x.Locker.CurrentCellLock is TileScoreCellLock)
                .Select(x => x.Locker.CurrentCellLock as TileScoreCellLock)
                .ToList();

            AddTileScoreCellLocks(tileScoreCellLocks);
        }

        private void LoadSimpleCellLocks(List<GridCell> lockedCells)
        {
            List<SimpleCellLock> simpleCellLocks = lockedCells
                .Where(x => x.Locker.CurrentCellLock is SimpleCellLock)
                .Select(x => x.Locker.CurrentCellLock as SimpleCellLock)
                .ToList();

            AddSimpleCellLocks(simpleCellLocks);
        }

        private void SubscribeUpdates()
        {
            _stackCompleter.StackCompleted += OnStackCompleted;
            _stackMerger.StackCompleted += OnStackCompleted;
        }

        private void CleanUp()
        {
            _stackCompleter.StackCompleted -= OnStackCompleted;
            _stackMerger.StackCompleted -= OnStackCompleted;
        }
    }
}