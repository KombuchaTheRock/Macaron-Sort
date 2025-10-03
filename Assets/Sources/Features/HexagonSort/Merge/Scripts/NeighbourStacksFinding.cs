using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class NeighbourStacksFinding
    {
        private readonly HexagonGrid _hexagonGrid;
        
        private Vector2Int[] _offsetsForOddRow =
        {
            new(1, 0), new(-1, 0), new(0, 1),
            new(1, 1), new(0, -1), new(1, -1)
        };

        private Vector2Int[] _offsetsForEvenRow =
        {
            new(1, 0), new(-1, 0), new(-1, 1),
            new(0, 1), new(-1, -1), new(0, -1)
        };

        public NeighbourStacksFinding(HexagonGrid hexagonGrid)
        {
            _hexagonGrid = hexagonGrid;
        }

        public List<GridCell> GetNeighboursByType(Vector2Int center, HexagonTileType topHexagonType)
        {
            List<GridCell> result = new();
            Vector2Int[] neighboursPositions = center.y % 2 == 0 ? _offsetsForEvenRow : _offsetsForOddRow;

            foreach (Vector2Int offset in neighboursPositions)
            {
                Vector2Int neighborPos = center + offset;

                if (_hexagonGrid.TryGetCell(neighborPos, out GridCell cell) == false)
                    continue;

                if (cell.IsOccupied && cell.Stack.TopHexagon.TileType == topHexagonType)
                    result.Add(cell);
            }

            return result;
        }
    }
}