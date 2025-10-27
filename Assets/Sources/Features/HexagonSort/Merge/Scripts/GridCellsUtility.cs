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

        public static List<GridCell> GetNeighboursByType(Vector2Int center, HexagonTileType topHexagonType,
            HexagonGrid grid)
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

        public static List<Vector2Int> GetRandomPositionOnEdge(HexagonGrid grid, float maxMagnitude)
        {
            List<Vector2Int> edgePositions = GetEdgePositions(grid);
            List<Vector2Int> randomPositionsOnEdge = new();

            foreach (Vector2Int edgePosition in edgePositions)
            {
                Vector2Int[] neighbours = GetNeighbourPositions(edgePosition);

                List<Vector2Int> neighboursOnEdge = neighbours
                    .Where(cell => grid.IsCellOnGrid(cell) == false &&
                                   grid.GridComponent
                                       .CellToWorld(new Vector3Int(cell.x, cell.y, 0))
                                       .magnitude < maxMagnitude)
                    .ToList();

                if (neighboursOnEdge.Count > 0)
                {
                    randomPositionsOnEdge.Add(neighboursOnEdge[Random.Range(0, neighboursOnEdge.Count - 1)]);
                }
            }

            return randomPositionsOnEdge;
        }

        public static bool TryGetRandomFreeCell(HexagonGrid grid, out GridCell gridCell)
        {
            GridCell[] freeCells = grid.Cells
                .Where(cell => cell is { IsOccupied: false, IsLocked: false })
                .ToArray();

            if (freeCells.Length <= 0)
            {
                gridCell = null;
                return false;
            }

            gridCell = freeCells[Random.Range(0, freeCells.Length - 1)];

            return true;
        }

        private static bool IsAllCellsOnGrid(Vector2Int[] cells, HexagonGrid grid) =>
            cells.All(grid.IsCellOnGrid);
        
        public static Vector3 CellToWorldWithRotation(Grid grid, Vector3Int cellPosition)
        {
            Vector3 localPosition = grid.CellToLocal(cellPosition);
            return grid.transform.TransformPoint(localPosition);
        }

        public static Vector3Int WorldToCellWithRotation(Grid grid, Vector3 worldPosition)
        {
            Vector3 localPosition = grid.transform.InverseTransformPoint(worldPosition);
            return grid.LocalToCell(localPosition);
        }
    }
}