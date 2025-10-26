using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
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

        private SortedSet<SimpleLockedCell> _simpleLockedCells = new();
        private IStackMerger _stackMerger;
        private IStackCompleter _stackCompleter;
        private IGameProgressService _progressService;

        [Inject]
        private void Construct(IStackMerger stackMerger, IStackCompleter stackCompleter)
        {
            _stackCompleter = stackCompleter;
            _stackMerger = stackMerger;
        }

        private void Awake() =>
            SubscribeUpdates();

        private void OnDestroy() =>
            CleanUp();

        private void OnStackCompleted(int obj)
        {
            if (_simpleLockedCells.Count <= 0)
                return;

            SimpleLockedCell simpleLockedCell = _simpleLockedCells
                .First();

            simpleLockedCell
                .SimpleCellLock
                .DecreaseCompletedStacksToUnlock();

            if (simpleLockedCell.SimpleCellLock.IsLocked == false) 
                _simpleLockedCells.Remove(simpleLockedCell);

            GridModified?.Invoke();
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

        public void AddRangeSimpleLockedCell(List<SimpleCellLock> simpleCellLocks) =>
            _simpleLockedCells.AddRange(simpleCellLocks.Select(x => new SimpleLockedCell(x)));

        public void ApplyProgress(GameProgress progress)
        {
            List<GridCell> lockedCells = _hexagonGrid.Cells
                .Where(cell => cell.IsLocked)
                .ToList();

            List<SimpleCellLock> simpleLockedCells = lockedCells
                .Where(x => x.Locker.CurrentCellLock is SimpleCellLock)
                .Select(x => x.Locker.CurrentCellLock as SimpleCellLock)
                .ToList();

            AddRangeSimpleLockedCell(simpleLockedCells);
        }
    }
}