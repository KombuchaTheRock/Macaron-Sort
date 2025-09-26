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

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSystem : MonoBehaviour, ICoroutineRunner
    {
        public event Action HexagonMergeAnimationCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        public event Action<int> StackCompleted;
        public event Action MergeStarted;
        public event Action MergeFinished;

        private HashSet<GridCell> _updatedCells = new();
        private StackMover _stackMover;
        private StackMergeLogic _mergeLogic;
        private StackMergeCandidate _completeCandidate;
        private HexagonGrid _hexagonGrid;

        public bool IsMerging { get; private set; }

        public void Initialize(StackMover stackMover, HexagonGrid hexagonGrid)
        {
            _hexagonGrid = hexagonGrid;
            _stackMover = stackMover;
            _mergeLogic = new StackMergeLogic(_hexagonGrid, this);
            
            SubscribeUpdates();

            UpdateOccupiedCells();
        }

        private void SubscribeUpdates()
        {
            _stackMover.StackPlaced += OnStackPlaced;
            _mergeLogic.StackCompleted += OnStackCompleted;
            _mergeLogic.MergeAnimationCompleted += HexagonMergeAnimationCompleted;
            _mergeLogic.DeleteAnimationCompleted += HexagonDeleteAnimationCompleted;
        }
        
        public void UpdateOccupiedCells()
        {
            List<GridCell> occupiedCells = _hexagonGrid.Cells.Where(cell => cell.IsOccupied).ToList();

            _updatedCells.AddRange(occupiedCells);
            StartCoroutine(CellsUpdatedRoutine());
        }

        private void OnStackCompleted(int score) =>
            StackCompleted?.Invoke(score);

        private void OnDestroy() => 
            CleanUp();

        private void CleanUp()
        {
            _stackMover.StackPlaced -= OnStackPlaced;
            _mergeLogic.StackCompleted -= OnStackCompleted;
            _mergeLogic.MergeAnimationCompleted -= HexagonMergeAnimationCompleted;
            _mergeLogic.DeleteAnimationCompleted -= HexagonDeleteAnimationCompleted;
        }

        private void OnStackPlaced(GridCell cell)
        {
            _updatedCells.Add(cell);

            if (IsMerging == false)
                StartCoroutine(CellsUpdatedRoutine());
        }

        private IEnumerator CellsUpdatedRoutine()
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
            List<GridCell> neighboursCells = _mergeLogic.GetSimilarNeighbourCells(cell.PositionOnGrid, topHexagonType);

            if (neighboursCells.Count <= 0)
                yield break;

            SortedSet<StackMergeCandidate> prioritizedNeighbourStacks =
                GetMergeCandidates(cell, neighboursCells);

            StackMergeCandidate placedStack = GetMergeCandidate(cell, topHexagonType);

            while (prioritizedNeighbourStacks.Count > 0)
            {
                StackMergeCandidate neighbourStack = prioritizedNeighbourStacks.First();

                SortedSet<StackMergeCandidate> mergePair = new()
                {
                    placedStack,
                    neighbourStack
                };

                bool isBothMonoStacks = mergePair.Min.IsMonoType && mergePair.Max.IsMonoType;

                StackMergeCandidate from;
                StackMergeCandidate to;

                if (prioritizedNeighbourStacks.Count > 1)
                {
                    from = neighbourStack;
                    to = placedStack;
                }
                else if (isBothMonoStacks)
                {
                    from = mergePair.Min;
                    to = mergePair.Max;
                }
                else
                {
                    from = mergePair.Max;
                    to = mergePair.Min;
                }

                _updatedCells.Add(from.Cell);
                _updatedCells.Add(to.Cell);


                List<Hexagon> hexagonsForMerge = _mergeLogic.GetHexagonsToMerge(topHexagonType, from.Stack);
                _mergeLogic.RemoveHexagonsFromStack(from.Stack, hexagonsForMerge);

                from.Stack.HideDisplayedSize();
                to.Stack.HideDisplayedSize();

                yield return StartCoroutine(_mergeLogic.MergeRoutine(to, hexagonsForMerge));

                if (from.Stack != null)
                    from.Stack.ShowDisplayedSize();

                prioritizedNeighbourStacks.Remove(neighbourStack);
                _completeCandidate = to;
            }

            yield return _mergeLogic.CheckStackForComplete(_completeCandidate);

            _completeCandidate.Stack.ShowDisplayedSize();
        }

        private SortedSet<StackMergeCandidate> GetMergeCandidates(GridCell filledCell,
            List<GridCell> neighboursCells)
        {
            SortedSet<StackMergeCandidate> stacksPriority = new();

            foreach (GridCell cell in neighboursCells)
            {
                StackMergeCandidate stack = GetMergeCandidate(cell, filledCell.Stack.TopHexagon.TileType);
                stacksPriority.Add(stack);
            }

            return stacksPriority;
        }

        private StackMergeCandidate GetMergeCandidate(GridCell cell, HexagonTileType topTile)
        {
            int sameHexagonCount = _mergeLogic.GetSimilarHexagons(cell.Stack, topTile, out bool isMonoType).Count;
            return new StackMergeCandidate(sameHexagonCount, cell.Stack, isMonoType, cell);
        }
    }
}