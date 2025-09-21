using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSystem : MonoBehaviour, ICoroutineRunner
    {
        private StackMover _stackMover;
        private StackMergeLogic _mergeLogic;

        public void Initialize(StackMover stackMover, HexagonGrid hexagonGrid)
        {
            _stackMover = stackMover;
            _stackMover.StackPlaced += OnStackPlaced;

            _mergeLogic = new StackMergeLogic(hexagonGrid);
        }

        private void OnDestroy() =>
            _stackMover.StackPlaced -= OnStackPlaced;

        private void OnStackPlaced(GridCell cell)
        {
            StartCoroutine(StackPlacedRoutine(cell));
        }

        private IEnumerator StackPlacedRoutine(GridCell cell)
        {
            HexagonStack placedStack = cell.Stack;
            List<GridCell> neighboursCells = _mergeLogic.GetNeighbourCells(cell.PositionOnGrid, placedStack.TopHexagon);

            if (neighboursCells.Count <= 0)
                yield break;

            SortedSet<StackWithPriority> prioritizedStacks =
                GetNeighbourStacksPriority(placedStack.TopHexagon, neighboursCells);
            
            prioritizedStacks.Add(GetStackPriority(placedStack, placedStack.TopHexagon));

            if (GetStacksForMerge(prioritizedStacks, out HexagonStack stackFrom, out HexagonStack stackTo) == false)
                yield break;

            List<Hexagon> hexagonsForMerge = _mergeLogic.GetHexagonsToMerge(placedStack.TopHexagon, stackFrom);
            _mergeLogic.RemoveHexagonsFromStack(stackFrom, hexagonsForMerge);

            yield return StartCoroutine(_mergeLogic.MergeRoutine(stackTo, hexagonsForMerge,
                () => StartCoroutine(_mergeLogic.CheckStackForComplete(placedStack))));
        }

        private bool GetStacksForMerge(SortedSet<StackWithPriority> prioritizedStacks, out HexagonStack sourceStack,
            out HexagonStack targetStack)
        {
            SeparateStacksByType(prioritizedStacks,
                out SortedSet<StackWithPriority> monoStacks,
                out SortedSet<StackWithPriority> diversityStacks);

            sourceStack = DetermineSourceStack(monoStacks, diversityStacks);
            targetStack = DetermineTargetStack(monoStacks, diversityStacks);

            return sourceStack != null && targetStack != null;
        }

        private void SeparateStacksByType(SortedSet<StackWithPriority> allStacks,
            out SortedSet<StackWithPriority> monoTypes,
            out SortedSet<StackWithPriority> diversityTypes)
        {
            monoTypes = new SortedSet<StackWithPriority>();
            diversityTypes = new SortedSet<StackWithPriority>();

            foreach (StackWithPriority stack in allStacks)
            {
                if (stack.IsMonoType)
                    monoTypes.Add(stack);
                else
                    diversityTypes.Add(stack);
            }
        }

        private HexagonStack DetermineSourceStack(SortedSet<StackWithPriority> monoTypes,
            SortedSet<StackWithPriority> diversityTypes)
        {
            bool hasBothTypes = monoTypes.Count > 0 && diversityTypes.Count > 0;
            bool hasOnlyMonoTypes = monoTypes.Count > 0;

            return hasBothTypes ? GetLowestPriorityStack(diversityTypes)
                : hasOnlyMonoTypes ? GetHighestPriorityStack(monoTypes)
                : GetHighestPriorityStack(diversityTypes);
        }

        private HexagonStack DetermineTargetStack(SortedSet<StackWithPriority> monoTypes, 
            SortedSet<StackWithPriority> diversityTypes)
        {
            return monoTypes.Count > 0 ? GetHighestPriorityStack(monoTypes) 
                : GetLowestPriorityStack(diversityTypes);
        }
        
        private HexagonStack GetHighestPriorityStack(SortedSet<StackWithPriority> stacks) => 
            ExtractStackFromSet(stacks, takeFirst: true);

        private HexagonStack GetLowestPriorityStack(SortedSet<StackWithPriority> stacks) => 
            ExtractStackFromSet(stacks, takeFirst: false);

        private HexagonStack ExtractStackFromSet(SortedSet<StackWithPriority> stacks, bool takeFirst)
        {
            if (stacks.Count == 0) return null;
    
            StackWithPriority stackWithPriority = takeFirst ? stacks.First() : stacks.Last();
            stacks.Remove(stackWithPriority);
    
            return stackWithPriority.Stack;
        }
        
        private SortedSet<StackWithPriority> GetNeighbourStacksPriority(HexagonTileType topTile,
            List<GridCell> neighboursCells)
        {
            SortedSet<StackWithPriority> stacksPriority = new();

            foreach (GridCell cell in neighboursCells)
            {
                StackWithPriority withPriority = GetStackPriority(cell.Stack, topTile);
                stacksPriority.Add(withPriority);
            }

            return stacksPriority;
        }

        private StackWithPriority GetStackPriority(HexagonStack stack, HexagonTileType topTile)
        {
            int sameHexagonCount = _mergeLogic.GetSimilarHexagons(stack, topTile, out bool isMonotype).Count;

            return new StackWithPriority(sameHexagonCount, stack, isMonotype);
        }
    }
}