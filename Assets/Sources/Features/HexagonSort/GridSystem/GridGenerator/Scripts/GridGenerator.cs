using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.GridSystem.Scripts;
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

        public HexagonGrid GenerateNewGrid(Grid grid, int gridSize, CellConfig cellConfig)
        {
            InitializeHexagonGrid(grid, cellConfig);
            
            List<GridCell> gridCells = GenerateNewGridCells(grid, gridSize, cellConfig);

            foreach (GridCell cell in gridCells)
                _hexagonGrid.AddCell(cell.PositionOnGrid, cell);
            
            return _hexagonGrid;
        }

        public HexagonGrid GenerateSavedGrid(Grid grid, CellConfig cellConfig, List<CellData> cellData)
        {
            InitializeHexagonGrid(grid, cellConfig);
            
            List<GridCell> gridCells = GenerateGridCellsFromData(grid, cellConfig, cellData);
            
            foreach (GridCell cell in gridCells)
                _hexagonGrid.AddCell(cell.PositionOnGrid, cell);
            
            return _hexagonGrid;
        }

        private void InitializeHexagonGrid(Grid grid, CellConfig cellConfig)
        {
            _hexagonGrid = _factory.CreateHexagonGrid();
            _hexagonGrid.Initialize(grid);
            _gridRoot = _hexagonGrid.transform;

            ConfigureGridComponent(grid, cellConfig);
        }

        private List<GridCell> GenerateGridCellsFromData(Grid grid, CellConfig cellConfig, List<CellData> cellData)
        {
            List<GridCell> gridCells = new();
            
            foreach (CellData cell in cellData)
            {
                Vector3 spawnPosition = grid.CellToWorld(new Vector3Int(cell.PositionOnGrid.x, cell.PositionOnGrid.y, 0));
                
                GridCell gridCell = GenerateGridCell(cell.PositionOnGrid, spawnPosition, cellConfig);
                gridCells.Add(gridCell);
            }
            
            return gridCells;
        }
        
        private List<GridCell> GenerateNewGridCells(Grid grid, int gridSize, CellConfig cellConfig)
        {
            List<GridCell> gridCells = new();
            
            int negativeGridSize = -gridSize;
            float maxGridRadius = grid.CellToWorld(Vector3Int.right).magnitude * gridSize;

            for (int x = negativeGridSize; x <= gridSize; x++)
            {
                for (int y = negativeGridSize; y <= gridSize; y++)
                {
                    if (IsPositionInRadius(grid, x, y, maxGridRadius, out Vector3 spawnPosition) == false) 
                        continue;
                    
                    GridCell cell = GenerateGridCell(new Vector2Int(x, y), spawnPosition, cellConfig);
                    gridCells.Add(cell);
                }
            }
            
            return gridCells;
        }

        public GridCell GenerateGridCell(Vector2Int positionOnGrid, Vector3 worldPosition, CellConfig cellConfig)
        {
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