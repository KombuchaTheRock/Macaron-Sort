
using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public static class NeighbourCellsFindingUtility
    {
        private static Vector2Int[] _offsetsForOddRow =
        {
            new(1, 0), new(-1, 0), new(0, 1),
            new(1, 1), new(0, -1), new(1, -1)
        };

        private static Vector2Int[] _offsetsForEvenRow =
        {
            new(1, 0), new(-1, 0), new(-1, 1),
            new(0, 1), new(-1, -1), new(0, -1)
        };

        public static List<GridCell> GetNeighboursByType(Vector2Int center, HexagonTileType topHexagonType, HexagonGrid grid)
        {
            List<GridCell> result = new();
            Vector2Int[] neighboursPositions = GetNeighbourPositions(center);
            
            foreach (Vector2Int neighboursPosition in neighboursPositions)
            {
                if (grid.TryGetCell(neighboursPosition, out GridCell cell) == false)
                    continue;

                if (cell.IsOccupied && cell.Stack.TopHexagon.TileType == topHexagonType)
                    result.Add(cell);
            }

            return result;
        }
        
        public static Vector2Int[] GetNeighbourPositions(Vector2Int center)
        {
            Vector2Int[] offsets = center.y % 2 == 0 ? _offsetsForEvenRow : _offsetsForOddRow;
            return offsets.Select(offset => center + offset).ToArray();
        }
    }
}