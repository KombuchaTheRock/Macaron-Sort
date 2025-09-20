using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

public class MergeSystem : MonoBehaviour
{
    private const int HexagonsCountForComplete = 10;
    private const int PriorityMonotypeBonus = 100;
    private StackMover _stackMover;
    private HexagonGrid _hexagonGrid;

    private Vector2Int[] _neighboursOddRow =
    {
        new(1, 0),
        new(-1, 0),
        new(0, 1),
        new(1, 1),
        new(0, -1),
        new(1, -1)
    };

    private Vector2Int[] _neighboursEvenRow =
    {
        new(1, 0),
        new(-1, 0),
        new(-1, 1),
        new(0, 1),
        new(-1, -1),
        new(0, -1)
    };

    public void Initialize(StackMover stackMover, HexagonGrid hexagonGrid)
    {
        _hexagonGrid = hexagonGrid;
        _stackMover = stackMover;
        _stackMover.StackPlaced += OnStackPlaced;
    }

    private void OnDestroy() =>
        _stackMover.StackPlaced -= OnStackPlaced;


    private class StackPriority
    {
        public int Priority;
        public HexagonStack Stack;

        public StackPriority(int priority, HexagonStack stack)
        {
            Priority = priority;
            Stack = stack;
        }
    }

    private void OnStackPlaced(GridCell occupiedCell)
    {
        HexagonStack placedStack = occupiedCell.Stack;
        
        List<GridCell> neighboursCells = GetNeighbourCells(occupiedCell.PositionOnGrid, placedStack.TopHexagon);

        if (neighboursCells.Count <= 0)
            return;

        List<StackPriority> neighbourStacksPriority = GetNeighbourStacksPriority(placedStack.TopHexagon, neighboursCells);

        foreach (StackPriority priority in neighbourStacksPriority) 
            Debug.Log($"Priority: {priority.Priority}");

        Debug.Log($"PlacedStack priority: {GetStackPriority(placedStack, placedStack.TopHexagon)}");
        
        // List<Hexagon> hexagonToMerge = GetHexagonToMerge(placedStack.TopHexagon, neighboursCells);
        // RemoveHexagonsFromNeighbours(neighboursCells, hexagonToMerge);
        //
        // MergeStacks(placedStack, hexagonToMerge);
        // CheckStackForComplete(placedStack);
    }

    private List<StackPriority> GetNeighbourStacksPriority(HexagonTileType topTile, List<GridCell> neighboursCells)
    {
        List<StackPriority> stacksPriority = new();

        foreach (GridCell cell in neighboursCells)
        {
            int stackPriority = GetStackPriority(cell.Stack, topTile);
            stacksPriority.Add(new StackPriority(stackPriority, cell.Stack));
        }

        return stacksPriority;
    }

    private int GetStackPriority(HexagonStack stack, HexagonTileType topTile)
    {
        int stackPriority = GetSimilarHexagons(stack, topTile, out bool isMonotype).Count;

        if (isMonotype) 
            stackPriority += PriorityMonotypeBonus;
        return stackPriority;
    }

    private void CheckStackForComplete(HexagonStack stack)
    {
        if (stack.Hexagons.Count < HexagonsCountForComplete)
            return;

        List<Hexagon> similarHexagons = GetSimilarHexagons(stack, stack.TopHexagon, out bool isMonotype);

        if (isMonotype && similarHexagons.Count < HexagonsCountForComplete)
            return;

        while (similarHexagons.Count > 0)
        {
            stack.Remove(similarHexagons[0]);

            Destroy(similarHexagons[0].gameObject);
            similarHexagons.RemoveAt(0);
        }
    }

    private static List<Hexagon> GetSimilarHexagons(HexagonStack stack, HexagonTileType sample, out bool isMonotype)
    {
        isMonotype = true;
        List<Hexagon> similarHexagons = new();

        for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
        {
            Hexagon hexagon = stack.Hexagons[i];

            if (hexagon.TileType != sample)
            {
                isMonotype = false;
                break;
            }

            similarHexagons.Add(hexagon);
        }

        return similarHexagons;
    }

    private static void MergeStacks(HexagonStack stack, List<Hexagon> hexagonToMerge)
    {
        float offsetBetweenTiles = stack.OffsetBetweenTiles;
        float initialY = (stack.Hexagons.Count + 1) * offsetBetweenTiles;

        for (int i = 0; i < hexagonToMerge.Count; i++)
        {
            Hexagon hexagon = hexagonToMerge[i];

            float targetY = initialY + i * offsetBetweenTiles;

            stack.Add(hexagon);
            hexagon.SetParent(stack.transform);
            hexagon.transform.position = new Vector3(stack.transform.position.x, targetY, stack.transform.position.z);
        }
    }

    private static void RemoveHexagonsFromNeighbours(List<GridCell> neighboursCells, List<Hexagon> hexagonToMerge)
    {
        foreach (GridCell cell in neighboursCells)
        {
            HexagonStack stack = cell.Stack;

            foreach (Hexagon hexagon in hexagonToMerge)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
            }
        }
    }

    private List<Hexagon> GetHexagonToMerge(HexagonTileType topHexagon, List<GridCell> neighboursCells)
    {
        List<Hexagon> hexagonToMerge = new();

        foreach (GridCell cell in neighboursCells)
        {
            HexagonStack stack = cell.Stack;

            for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = stack.Hexagons[i];

                if (hexagon.TileType != topHexagon)
                    break;

                hexagonToMerge.Add(stack.Hexagons[i]);
                hexagon.SetParent(null);
            }
        }

        return hexagonToMerge;
    }

    private List<GridCell> GetNeighbourCells(Vector2Int centerPosition, HexagonTileType topHexagonType)
    {
        List<GridCell> result = new();

        Vector2Int[] neighbours = centerPosition.y % 2 == 0 ? _neighboursEvenRow : _neighboursOddRow;

        foreach (Vector2Int offset in neighbours)
        {
            Vector2Int neighborPos = centerPosition + offset;

            if (_hexagonGrid.TryGetCell(neighborPos, out GridCell cell) == false)
                continue;

            if (cell.IsOccupied && cell.Stack.TopHexagon == topHexagonType)
                result.Add(cell);
        }

        return result;
    }
}