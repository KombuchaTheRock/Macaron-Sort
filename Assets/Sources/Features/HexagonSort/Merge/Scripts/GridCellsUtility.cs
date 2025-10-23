
using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public static class GridCellsUtility
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
        
        private static Vector2Int[][] _oppositePairsForEvenRow =
        {
            new[] { new Vector2Int(0, 1), new Vector2Int(-1, -1) },   
            new[] { new Vector2Int(-1, 1), new Vector2Int(0, -1) },    
            new[] { new Vector2Int(-1, 0), new Vector2Int(1, 0) }      
        };

        private static Vector2Int[][] _oppositePairsForOddRow =
        {
            new[] { new Vector2Int(0, 1), new Vector2Int(1, -1) },    
            new[] { new Vector2Int(1, 1), new Vector2Int(0, -1) },     
            new[] { new Vector2Int(-1, 0), new Vector2Int(1, 0) }      
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

        public static int GetCellOppositeNeighbourPairCount(Vector2Int center, HexagonGrid grid)
        {
            bool isCellInEvenRow = IsCellInEvenRow(center);
            Vector2Int[][] oppositePairs = isCellInEvenRow ? _oppositePairsForOddRow : _oppositePairsForEvenRow;

            return oppositePairs.Count(oppositePair => IsAllCellsOnGrid(oppositePair, grid));
        }
        
        public static Vector2Int[] GetNeighbourPositions(Vector2Int center)
        {
            Vector2Int[] offsets = IsCellInEvenRow(center) ? _offsetsForEvenRow : _offsetsForOddRow;
            return offsets.Select(offset => center + offset).ToArray();
        }

        private static bool IsCellInEvenRow(Vector2Int center) => 
            center.y % 2 == 0;

        public static List<Vector2Int> GetEdgePositions(HexagonGrid grid)
        {
            List<Vector2Int> edgePositions = new();

            foreach (Vector2Int positionOnGrid in grid.Cells.Select(cell => cell.PositionOnGrid))
            {
                Vector2Int[] neighbours = GetNeighbourPositions(positionOnGrid);

                if (IsAllCellsOnGrid(neighbours, grid) == false)
                    edgePositions.Add(positionOnGrid);
            }

            return edgePositions;
        }
        
        private static bool IsAllCellsOnGrid(Vector2Int[] cells, HexagonGrid grid) =>
            cells.All(grid.IsCellOnGrid);
    }
}