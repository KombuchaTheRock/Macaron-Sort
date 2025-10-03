using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
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

        private HashSet<GridCell> _updatedCells = new();
        private IStackMover _stackMover;
        private MergeCandidate _completeCandidate;
        private HexagonGrid _hexagonGrid;

        private StackMerge _merge;
        private NeighbourStacksFinding _neighbourFinding;
        private StackCompletion _stackCompletion;
        private MergePriority _mergePriority;

        public bool IsMerging { get; private set; }

        [Inject]
        private void Construct(IStackMover stackMover)
        {
            _stackMover = stackMover;
        }

        public void Initialize(HexagonGrid hexagonGrid)
        {
            MergeAnimation mergeAnimation = new();

            _hexagonGrid = hexagonGrid;
            _merge = new StackMerge(mergeAnimation);
            _stackCompletion = new StackCompletion(10);
            _neighbourFinding = new NeighbourStacksFinding(hexagonGrid);
            _mergePriority = new MergePriority();

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
                    yield return CheckForMergeRoutine(_updatedCells.First());
            }
            finally
            {
                IsMerging = false;
                MergeFinished?.Invoke();
            }
        }

        private IEnumerator CheckForMergeRoutine(GridCell cell)
        {
            _updatedCells.Remove(cell);
        
            if (cell.IsOccupied == false)
                yield break;
        
            HexagonTileType topHexagonType = cell.Stack.TopHexagon.TileType;
            List<GridCell> neighboursCells = _neighbourFinding.GetNeighboursByType(cell.PositionOnGrid, topHexagonType);
        
            if (neighboursCells.Count <= 0)
                yield break;
        
            SortedSet<MergeCandidate> neighbourStacks = _mergePriority.GetMergeCandidates(neighboursCells);
            MergeCandidate placed = new(cell);
        
            while (neighbourStacks.Count > 0)
            {
                MergeCandidate neighbour = neighbourStacks.First();
        
                (MergeCandidate from, MergeCandidate to) = _mergePriority.GetMergePair(placed, neighbour, neighbourStacks.Count);
        
                _updatedCells.Add(from.Cell);
                _updatedCells.Add(to.Cell);
        
                List<Hexagon> hexagonsForMerge = StackAnalyze.GetSimilarHexagons(from.Stack, topHexagonType);
        
                from.Stack.HideDisplayedSize();
                to.Stack.HideDisplayedSize();
        
                yield return StartCoroutine(_merge.MergeRoutine(to, hexagonsForMerge));
        
                StackAnalyze.RemoveHexagonsFromStack(from.Stack, hexagonsForMerge);
                StackAnalyze.AddHexagonsToStack(to.Stack, hexagonsForMerge);
        
                if (from.Stack != null)
                    from.Stack.ShowDisplayedSize();
        
                neighbourStacks.Remove(neighbour);
                _completeCandidate = to;
            }
        
            yield return StartCoroutine(_stackCompletion.CheckStackForCompleteRoutine(_completeCandidate));
        
            _completeCandidate.Stack.ShowDisplayedSize();
        }

        private void SubscribeUpdates()
        {
            _stackMover.StackPlaced += OnStackPlaced;

            _stackCompletion.StackCompleted += OnStackCompleted;
            _stackCompletion.DeleteAnimationCompleted += HexagonDeleteAnimationCompleted;

            _merge.MergeAnimationCompleted += MergeAnimationCompleted;
        }

        private void CleanUp()
        {
            _stackMover.StackPlaced -= OnStackPlaced;
            _merge.MergeAnimationCompleted -= MergeAnimationCompleted;

            _stackCompletion.StackCompleted -= OnStackCompleted;
            _stackCompletion.DeleteAnimationCompleted -= HexagonDeleteAnimationCompleted;
        }
    }
}