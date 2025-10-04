using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeLogic
    {
        public event Action MergeAnimationCompleted;
        public event Action HexagonDeleteAnimationCompleted;
        public event Action<int> StackCompleted;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly HashSet<GridCell> _updatedCells;
        private readonly StackMerge _merge;
        private readonly StackCompletion _stackCompletion;
        private readonly NeighbourStacksFinding _neighbourFinding;
        private readonly MergePriority _mergePriority;

        public MergeLogic(ICoroutineRunner coroutineRunner, HexagonGrid hexagonGrid,
            HashSet<GridCell> updatedCells)
        {
            MergeAnimation mergeAnimation = new();

            _coroutineRunner = coroutineRunner;
            _updatedCells = updatedCells;

            _merge = new StackMerge(mergeAnimation);
            _stackCompletion = new StackCompletion(10);
            _neighbourFinding = new NeighbourStacksFinding(hexagonGrid);
            _mergePriority = new MergePriority();
            
            SubscribeUpdates();
        }

        public void CleanUp()
        {
            _stackCompletion.StackCompleted -= OnStackCompleted;
            _stackCompletion.DeleteAnimationCompleted -= OnDeleteAnimationCompleted;
            
            _merge.MergeAnimationCompleted -= OnMergeAnimationCompleted;
        }

        public IEnumerator CheckForMergeRoutine(GridCell cell)
        {
            if (!ShouldProcessCell(cell))
                yield break;

            MergeContext mergeContext = PrepareMergeContext(cell);
            if (!mergeContext.HasNeighbours)
                yield break;

            yield return ProcessAllMerges(mergeContext);
            yield return FinalizeMerge(mergeContext);
        }

        private bool ShouldProcessCell(GridCell cell)
        {
            _updatedCells.Remove(cell);
            return cell.IsOccupied;
        }

        private MergeContext PrepareMergeContext(GridCell cell)
        {
            HexagonTileType topHexagonType = cell.Stack.TopHexagon.TileType;
            List<GridCell> neighboursCells = _neighbourFinding.GetNeighboursByType(cell.PositionOnGrid, topHexagonType);
            SortedSet<MergeCandidate> neighbourStacks = _mergePriority.GetMergeCandidates(neighboursCells);

            return new MergeContext
            {
                Cell = cell,
                TopHexagonType = topHexagonType,
                NeighbourStacks = neighbourStacks,
                PlacedCandidate = new MergeCandidate(cell)
            };
        }

        private IEnumerator ProcessAllMerges(MergeContext context)
        {
            while (context.NeighbourStacks.Count > 0)
            {
                yield return ProcessSingleMerge(context);
            }
        }

        private IEnumerator ProcessSingleMerge(MergeContext context)
        {
            MergeCandidate neighbour = context.NeighbourStacks.First();
            (MergeCandidate from, MergeCandidate to) =
                _mergePriority.GetMergePair(context.PlacedCandidate, neighbour, context.NeighbourStacks.Count);

            UpdateProcessedCells(from, to);

            List<Hexagon> hexagonsForMerge = HexagonStackUtils.GetSimilarHexagons(from.Stack, context.TopHexagonType);

            HideStackSizes(from, to);
            yield return ExecuteMerge(to, hexagonsForMerge);
            TransferHexagons(from, to, hexagonsForMerge);
            ShowStackSizeIfExists(from);

            context.NeighbourStacks.Remove(neighbour);
            context.CompleteCandidate = to;
        }

        private void UpdateProcessedCells(MergeCandidate from, MergeCandidate to)
        {
            _updatedCells.Add(from.Cell);
            _updatedCells.Add(to.Cell);
        }

        private void HideStackSizes(MergeCandidate from, MergeCandidate to)
        {
            from.Stack.HideDisplayedSize();
            to.Stack.HideDisplayedSize();
        }

        private IEnumerator ExecuteMerge(MergeCandidate to, List<Hexagon> hexagonsForMerge)
        {
            yield return _coroutineRunner.StartCoroutine(_merge.MergeRoutine(to, hexagonsForMerge));
        }

        private void TransferHexagons(MergeCandidate from, MergeCandidate to, List<Hexagon> hexagons)
        {
            foreach (Hexagon hexagon in hexagons.Where(from.Stack.Contains))
            {
                from.Stack.Remove(hexagon);
                to.Stack.Add(hexagon);
            }
        }

        private void ShowStackSizeIfExists(MergeCandidate candidate)
        {
            if (candidate.Stack != null)
                candidate.Stack.ShowDisplayedSize();
        }

        private IEnumerator FinalizeMerge(MergeContext context)
        {
            yield return _coroutineRunner.StartCoroutine(
                _stackCompletion.CheckStackForCompleteRoutine(context.CompleteCandidate));
            context.CompleteCandidate.Stack.ShowDisplayedSize();
        }

        private void SubscribeUpdates()
        {
            _stackCompletion.StackCompleted += OnStackCompleted;
            _stackCompletion.DeleteAnimationCompleted += OnDeleteAnimationCompleted;
            
            _merge.MergeAnimationCompleted += OnMergeAnimationCompleted;
        }

        private void OnDeleteAnimationCompleted() => 
            HexagonDeleteAnimationCompleted?.Invoke();

        private void OnMergeAnimationCompleted() => 
            MergeAnimationCompleted?.Invoke();

        private void OnStackCompleted(int score) => 
            StackCompleted?.Invoke(score);
        
        private class MergeContext
        {
            public GridCell Cell { get; set; }
            public HexagonTileType TopHexagonType { get; set; }
            public SortedSet<MergeCandidate> NeighbourStacks { get; set; }
            public MergeCandidate PlacedCandidate { get; set; }
            public MergeCandidate CompleteCandidate { get; set; }
            public bool HasNeighbours => NeighbourStacks?.Count > 0;
        }
    }
}