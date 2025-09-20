using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using UnityEngine;

public class MergeSystem : MonoBehaviour
{
    private const int MaxNeighboursCount = 6;
    
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

    private List<GridCell> _neighboursCells = new(MaxNeighboursCount);

    public void Initialize(StackMover stackMover, HexagonGrid hexagonGrid)
    {
        _hexagonGrid = hexagonGrid;
        _stackMover = stackMover;
        _stackMover.StackPlaced += OnStackPlaced;
    }

    private void OnDestroy() =>
        _stackMover.StackPlaced -= OnStackPlaced;

    private void OnStackPlaced(Vector2Int position)
    {
        if (_neighboursCells.Count > 0)
        {
            foreach (GridCell gridCell in _neighboursCells) 
                gridCell.DisableHighlight();
            
            _neighboursCells.Clear();
        }
        
        _neighboursCells = GetNeighbourCells(position);
        
        foreach (GridCell gridCell in _neighboursCells) 
            gridCell.EnableHighlight();
    }
    
    private List<GridCell> GetNeighbourCells(Vector2Int centerPosition)
    {
        List<GridCell> result = new();
        
        Vector2Int[] neighbours = centerPosition.y % 2 == 0 ? _neighboursEvenRow : _neighboursOddRow;
    
        foreach (Vector2Int offset in neighbours)
        {
            Vector2Int neighborPos = centerPosition + offset;

            if (_hexagonGrid.TryGetCell(neighborPos, out GridCell cell) == false) 
                continue;
            
            if (cell.IsOccupied)
                result.Add(cell);
        }
        
        return result; 
    }
}