using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackMerger : IStackMerger, IDisposable
    {
        public event Action MergeAnimationCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        public event Action<int> StackCompleted;
        public event Action MergeStarted;
        public event Action MergeFinished;

        private HexagonGrid _hexagonGrid;
        private MergeLogic _mergeLogic;
        private HashSet<GridCell> _updatedCells = new();
        
        private readonly IStackMover _stackMover;
        private readonly ICoroutineRunner _coroutineRunner;

        public bool IsMerging { get; private set; }

        public StackMerger(IStackMover stackMover, ICoroutineRunner coroutineRunner)
        {
            _stackMover = stackMover;
            _coroutineRunner = coroutineRunner;
        }

        public void Initialize(HexagonGrid hexagonGrid)
        {
            _hexagonGrid = hexagonGrid;
            _mergeLogic = new MergeLogic(_coroutineRunner, _hexagonGrid, _updatedCells);

            SubscribeUpdates();
            UpdateOccupiedCells();
        }

        public void Dispose() => 
            CleanUp();

        public void UpdateOccupiedCells()
        {
            List<GridCell> occupiedCells = _hexagonGrid.Cells
                .Where(cell => cell.IsOccupied)
                .ToList();

            _updatedCells.AddRange(occupiedCells);
            _coroutineRunner.StartCoroutine(CheckUpdatedCellsRoutine());
        }

        private void OnStackCompleted(int score) =>
            StackCompleted?.Invoke(score);

        private void OnStackPlaced(GridCell cell)
        {
            Debug.Log("OnStackPlaced");
            
            _updatedCells.Add(cell);

            if (IsMerging == false)
                _coroutineRunner.StartCoroutine(CheckUpdatedCellsRoutine());
        }

        private IEnumerator CheckUpdatedCellsRoutine()
        {
            IsMerging = true;
            MergeStarted?.Invoke();

            try
            {
                while (_updatedCells.Count > 0)
                    yield return _coroutineRunner.StartCoroutine(_mergeLogic.CheckForMergeRoutine(_updatedCells.First()));
            }
            finally
            {
                IsMerging = false;
                MergeFinished?.Invoke();
            }
        }

        private void SubscribeUpdates()
        {
            _stackMover.StackPlaced += OnStackPlaced;

            _mergeLogic.StackCompleted += OnStackCompleted;
            _mergeLogic.HexagonDeleteAnimationCompleted += HexagonDeleteAnimationCompleted;
            _mergeLogic.MergeAnimationCompleted += MergeAnimationCompleted;
        }

        private void CleanUp()
        {
            _stackMover.StackPlaced -= OnStackPlaced;

            _mergeLogic.CleanUp();
            _mergeLogic.StackCompleted -= OnStackCompleted;
            _mergeLogic.HexagonDeleteAnimationCompleted -= HexagonDeleteAnimationCompleted;
            _mergeLogic.MergeAnimationCompleted -= MergeAnimationCompleted;
        }
    }
}