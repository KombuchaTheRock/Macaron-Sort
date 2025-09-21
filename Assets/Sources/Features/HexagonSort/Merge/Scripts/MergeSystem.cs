using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSystem : MonoBehaviour, ICoroutineRunner
    {
        private List<GridCell> _updatedCells  = new();
        private StackMover _stackMover;
        private StackMergeLogic _mergeLogic;

        public void Initialize(StackMover stackMover, HexagonGrid hexagonGrid)
        {
            _stackMover = stackMover;
            _stackMover.StackPlaced += OnStackPlaced;

            _mergeLogic = new StackMergeLogic(hexagonGrid, this);
        }

        private void OnDestroy() =>
            _stackMover.StackPlaced -= OnStackPlaced;

        private void OnStackPlaced(GridCell cell) => 
            StartCoroutine(StackPlacedRoutine(cell));

        private IEnumerator StackPlacedRoutine(GridCell cell)
        {
           _updatedCells.Add(cell);

           while (_updatedCells.Count > 0)
               yield return CheckForMergeRoutine(_updatedCells.First());
        }

        private IEnumerator CheckForMergeRoutine(GridCell cell)
        {
            _updatedCells.Remove(cell);

            if (cell.IsOccupied == false)
                yield break;

            HexagonTileType topHexagonType = cell.Stack.TopHexagon;
            
            List<GridCell> neighboursCells = _mergeLogic.GetSimilarNeighbourCells(cell.PositionOnGrid, topHexagonType);

            if (neighboursCells.Count <= 0)
                yield break;

            SortedSet<StackMergeCandidate> prioritizedStacks =
                GetStacksPriority(cell, neighboursCells);

            while (prioritizedStacks.Count > 0)
            {
                if (GetStacksForMerge(prioritizedStacks, out StackMergeCandidate sourceStack, out StackMergeCandidate targetStack))
                {
                    _updatedCells.Add(sourceStack.Cell);
                    _updatedCells.Add(targetStack.Cell);
                
                    List<Hexagon> hexagonsForMerge = _mergeLogic.GetHexagonsToMerge(topHexagonType, sourceStack.Stack);
                    _mergeLogic.RemoveHexagonsFromStack(sourceStack.Stack, hexagonsForMerge);
            
                    yield return StartCoroutine(_mergeLogic.MergeRoutine(targetStack, hexagonsForMerge));
                }
            }
        }

        private bool GetStacksForMerge(SortedSet<StackMergeCandidate> prioritizedStacks, out StackMergeCandidate sourceStack,
            out StackMergeCandidate targetStack)
        {
            SeparateStacksByType(prioritizedStacks,
                out SortedSet<StackMergeCandidate> monoStacks,
                out SortedSet<StackMergeCandidate> diversityStacks);

            sourceStack = DetermineSourceStack(monoStacks, diversityStacks);
            targetStack = DetermineTargetStack(monoStacks, diversityStacks);
            
            prioritizedStacks.Remove(sourceStack);
            prioritizedStacks.Remove(targetStack);

            return sourceStack != null && targetStack != null;
        }

        private void SeparateStacksByType(SortedSet<StackMergeCandidate> allStacks,
            out SortedSet<StackMergeCandidate> monoTypes,
            out SortedSet<StackMergeCandidate> diversityTypes)
        {
            monoTypes = new SortedSet<StackMergeCandidate>();
            diversityTypes = new SortedSet<StackMergeCandidate>();

            foreach (StackMergeCandidate stack in allStacks)
            {
                if (stack.IsMonoType)
                    monoTypes.Add(stack);
                else
                    diversityTypes.Add(stack);
            }
        }

        private StackMergeCandidate DetermineSourceStack(SortedSet<StackMergeCandidate> monoTypes,
            SortedSet<StackMergeCandidate> diversityTypes)
        {
            bool hasBothTypes = monoTypes.Count > 0 && diversityTypes.Count > 0;
            bool hasOnlyMonoTypes = monoTypes.Count > 0;

            return hasBothTypes ? GetLowestPriorityStack(diversityTypes)
                : hasOnlyMonoTypes ? GetHighestPriorityStack(monoTypes)
                : GetHighestPriorityStack(diversityTypes);
        }

        private StackMergeCandidate DetermineTargetStack(SortedSet<StackMergeCandidate> monoTypes, 
            SortedSet<StackMergeCandidate> diversityTypes)
        {
            return monoTypes.Count > 0 ? GetHighestPriorityStack(monoTypes) 
                : GetLowestPriorityStack(diversityTypes);
        }
        
        private StackMergeCandidate GetHighestPriorityStack(SortedSet<StackMergeCandidate> stacks) => 
            ExtractStackFromSet(stacks, takeFirst: true);

        private StackMergeCandidate GetLowestPriorityStack(SortedSet<StackMergeCandidate> stacks) => 
            ExtractStackFromSet(stacks, takeFirst: false);

        private StackMergeCandidate ExtractStackFromSet(SortedSet<StackMergeCandidate> stacks, bool takeFirst)
        {
            if (stacks.Count == 0) return null;
    
            StackMergeCandidate stackMergeCandidate = takeFirst ? stacks.First() : stacks.Last();
            stacks.Remove(stackMergeCandidate);
    
            return stackMergeCandidate;
        }
        
        private SortedSet<StackMergeCandidate> GetStacksPriority(GridCell filledCell,
            List<GridCell> neighboursCells)
        {
            SortedSet<StackMergeCandidate> stacksPriority = new();

            foreach (GridCell cell in neighboursCells)
            {
                StackMergeCandidate stack = GetMergeCandidate(cell, filledCell.Stack.TopHexagon);
                stacksPriority.Add(stack);
            }

            StackMergeCandidate placedStackPriority = GetMergeCandidate(filledCell, filledCell.Stack.TopHexagon);
            stacksPriority.Add(placedStackPriority);
            
            return stacksPriority;
        }

        private StackMergeCandidate GetMergeCandidate(GridCell cell, HexagonTileType topTile)
        {
            int sameHexagonCount = _mergeLogic.GetSimilarHexagons(cell.Stack, topTile, out bool isMonoType).Count;

            return new StackMergeCandidate(sameHexagonCount, cell.Stack, isMonoType, cell);
        }
    }
}