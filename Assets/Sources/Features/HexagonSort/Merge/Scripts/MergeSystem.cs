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
using Zenject;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSystem : MonoBehaviour, ICoroutineRunner
    {
        public event Action MergeAnimationCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        public event Action<int> StackCompleted;
        public event Action MergeStarted;
        public event Action MergeFinished;

        private IStackMover _stackMover;
        private HexagonGrid _hexagonGrid;
        private MergeLogic _mergeLogic;
        private HashSet<GridCell> _updatedCells = new();

        public bool IsMerging { get; private set; }

        [Inject]
        private void Construct(IStackMover stackMover) => 
            _stackMover = stackMover;

        public void Initialize(HexagonGrid hexagonGrid)
        {
            _hexagonGrid = hexagonGrid;
            _mergeLogic = new MergeLogic(this, hexagonGrid, _updatedCells);

            SubscribeUpdates();
            UpdateOccupiedCells();
        }

        public void UpdateOccupiedCells()
        {
            List<GridCell> occupiedCells = _hexagonGrid.Cells.Where(cell => cell.IsOccupied).ToList();

            _updatedCells.AddRange(occupiedCells);
            StartCoroutine(CheckUpdatedCellsRoutine());
        }

        private void OnStackCompleted(int score) =>
            StackCompleted?.Invoke(score);

        private void OnDestroy() =>
            CleanUp();

        private void OnStackPlaced(GridCell cell)
        {
            _updatedCells.Add(cell);

            if (IsMerging == false)
                StartCoroutine(CheckUpdatedCellsRoutine());
        }

        private IEnumerator CheckUpdatedCellsRoutine()
        {
            IsMerging = true;
            MergeStarted?.Invoke();

            try
            {
                while (_updatedCells.Count > 0)
                    yield return StartCoroutine(_mergeLogic.CheckForMergeRoutine(_updatedCells.First()));
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
            _mergeLogic.CleanUp();
            
            _stackMover.StackPlaced -= OnStackPlaced;

            _mergeLogic.StackCompleted -= OnStackCompleted;
            _mergeLogic.HexagonDeleteAnimationCompleted -= HexagonDeleteAnimationCompleted;
            _mergeLogic.MergeAnimationCompleted -= MergeAnimationCompleted;
        }
    }
}