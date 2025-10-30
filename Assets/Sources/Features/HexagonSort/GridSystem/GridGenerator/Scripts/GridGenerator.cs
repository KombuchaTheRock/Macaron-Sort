using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.GridSystem.GridModificator.Scripts.CellLock;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public class GridGenerator : IGridGenerator
    {
        private readonly IGameFactory _factory;
        private Transform _gridRoot;
        private HexagonGrid _hexagonGrid;

        public GridGenerator(IGameFactory gameFactory) =>
            _factory = gameFactory;

        public HexagonGrid GenerateNewGrid(int gridSize, CellConfig cellConfig)
        {
            InitializeHexagonGrid(cellConfig);

            List<GridCell> gridCells = GenerateNewGridCells(gridSize, cellConfig);

            foreach (GridCell cell in gridCells)
                _hexagonGrid.AddCell(cell.PositionOnGrid, cell);

            return _hexagonGrid;
        }

        public HexagonGrid GenerateSavedGrid(CellConfig cellConfig, List<CellData> cellData)
        {
            InitializeHexagonGrid(cellConfig);

            List<GridCell> gridCells = GenerateGridCellsFromData(cellConfig, cellData);

            foreach (GridCell cell in gridCells)
                _hexagonGrid.AddCell(cell.PositionOnGrid, cell);

            return _hexagonGrid;
        }

        private void InitializeHexagonGrid(CellConfig cellConfig)
        {
            _hexagonGrid = _factory.CreateHexagonGrid();
            _gridRoot = _hexagonGrid.transform;

            ConfigureGridComponent(_hexagonGrid.GridComponent, cellConfig);
        }

        private List<GridCell> GenerateGridCellsFromData(CellConfig cellConfig, List<CellData> cellDataList)
        {
            List<GridCell> gridCells = new();

            foreach (CellData cellData in cellDataList)
            {
                GridCell gridCell = GenerateCellFromData(cellConfig, cellData);
                gridCells.Add(gridCell);
            }

            return gridCells;
        }

        private GridCell GenerateCellFromData(CellConfig cellConfig, CellData cellData)
        {
            GridCell gridCell = GenerateGridCell(cellData.PositionOnGrid, cellConfig);

            if (cellData.IsLocked == false) 
                return gridCell;
            
            CellLock cellLock = cellData.LockType switch
            {
                CellLockType.Simple => CellLock.FromData(cellData.SimpleLockData),
                CellLockType.TileScore => CellLock.FromData(cellData.TileScoreLockData),
                _ => throw new ArgumentOutOfRangeException()
            };

            gridCell.Lock(cellLock);

            return gridCell;
        }

        private List<GridCell> GenerateNewGridCells(int gridSize, CellConfig cellConfig)
        {
            List<GridCell> gridCells = new();

            int negativeGridSize = -gridSize;
            float maxGridRadius = _hexagonGrid.GridComponent.CellToWorld(Vector3Int.right).magnitude * gridSize;

            for (int x = negativeGridSize; x <= gridSize; x++)
            {
                for (int y = negativeGridSize; y <= gridSize; y++)
                {
                    if (IsPositionInRadius(_hexagonGrid.GridComponent, x, y, maxGridRadius, out Vector3 spawnPosition) == false)
                        continue;

                    GridCell cell = GenerateGridCell(new Vector2Int(x, y), cellConfig);
                    gridCells.Add(cell);
                }
            }

            return gridCells;
        }

        public GridCell GenerateGridCell(Vector2Int positionOnGrid, CellConfig cellConfig)
        {
            Vector3 localCellPosition = _hexagonGrid.GridComponent.CellToLocal(
                new Vector3Int(positionOnGrid.x, positionOnGrid.y, 0));
    
            Vector3 worldPosition = _hexagonGrid.GridComponent.transform.TransformPoint(localCellPosition);
            
            GridCell gridCell =
                _factory.CreateGridCell(worldPosition, positionOnGrid, _gridRoot, cellConfig.CellColor,
                    cellConfig.CellHighlightColor);
            gridCell.gameObject.name = $"({positionOnGrid.x}, {positionOnGrid.y})";

            return gridCell;
        }

        private static void ConfigureGridComponent(Grid grid, CellConfig cellConfig)
        {
            float inradius = GeometryUtils.InradiusFromOutRadius(cellConfig.CellSize);

            grid.cellSize = new Vector3(inradius, cellConfig.CellSize, 1f);
            grid.cellSwizzle = GridLayout.CellSwizzle.XZY;
        }

        private bool IsPositionInRadius(Grid grid, int x, int y, float maxGridRadius, out Vector3 spawnPosition)
        {
            spawnPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
            return spawnPosition.magnitude <= maxGridRadius;
        }
    }
}