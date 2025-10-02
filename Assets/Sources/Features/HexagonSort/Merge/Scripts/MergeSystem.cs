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
        public event Action HexagonMergeAnimationCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        public event Action<int> StackCompleted;
        public event Action MergeStarted;
        public event Action MergeFinished;

        private HashSet<GridCell> _updatedCells = new();
        private IStackMover _stackMover;
        private StackMergeLogic _mergeLogic;
        private MergeCandidate _completeCandidate;
        private HexagonGrid _hexagonGrid;

        public bool IsMerging { get; private set; }

        [Inject]
        private void Construct(IStackMover stackMover)
        {
            _stackMover = stackMover;
        }

        public void Initialize(HexagonGrid hexagonGrid)
        {
            _hexagonGrid = hexagonGrid;
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

            SortedSet<MergeCandidate> neighbourStacks =
                GetMergeCandidates(cell, neighboursCells);

            MergeCandidate placed = GetMergeCandidate(cell, topHexagonType);

            while (neighbourStacks.Count > 0)
            {
                MergeCandidate neighbour = neighbourStacks.First();

                (MergeCandidate from, MergeCandidate to) = GetMergePair(placed, neighbour, neighbourStacks.Count);

                _updatedCells.Add(from.Cell);
                _updatedCells.Add(to.Cell);

                List<Hexagon> hexagonsFrom = _mergeLogic.GetHexagonsToMerge(topHexagonType, from.Stack);

                from.Stack.HideDisplayedSize();
                to.Stack.HideDisplayedSize();

                yield return StartCoroutine(_mergeLogic.MergeRoutine(to, hexagonsFrom));

                _mergeLogic.RemoveHexagonsFromStack(from.Stack, hexagonsFrom);
                _mergeLogic.AddHexagonsToStack(to.Stack, hexagonsFrom);

                if (from.Stack != null)
                    from.Stack.ShowDisplayedSize();

                neighbourStacks.Remove(neighbour);
                _completeCandidate = to;
            }

            yield return _mergeLogic.CheckStackForComplete(_completeCandidate);
            
            _completeCandidate.Stack.ShowDisplayedSize();
        }

        private (MergeCandidate from, MergeCandidate to) GetMergePair(MergeCandidate placed, MergeCandidate neighbour,
            int neighbourCount)
        {
            SortedSet<MergeCandidate> mergePair = new()
            {
                placed,
                neighbour
            };

            MergeCandidate from;
            MergeCandidate to;

            if (neighbourCount > 1)
            {
                from = neighbour;
                to = placed;
            }
            else if (mergePair.Max.IsMonoType)
            {
                from = mergePair.Min;
                to = mergePair.Max;
            }
            else
            {
                from = mergePair.Max;
                to = mergePair.Min;
            }

            return (from, to);
        }

        private SortedSet<MergeCandidate> GetMergeCandidates(GridCell filledCell,
            List<GridCell> neighboursCells)
        {
            SortedSet<MergeCandidate> stacksPriority = new();

            foreach (GridCell cell in neighboursCells)
            {
                MergeCandidate stack = GetMergeCandidate(cell, filledCell.Stack.TopHexagon.TileType);
                stacksPriority.Add(stack);
            }

            return stacksPriority;
        }

        private MergeCandidate GetMergeCandidate(GridCell cell, HexagonTileType topTile)
        {
            int sameHexagonCount = _mergeLogic.GetSimilarHexagons(cell.Stack, topTile, out bool isMonoType).Count;
            return new MergeCandidate(sameHexagonCount, cell.Stack, isMonoType, cell);
        }
    }
}