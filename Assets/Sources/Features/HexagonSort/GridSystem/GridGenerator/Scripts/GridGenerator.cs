using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
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

        public HexagonGrid GenerateGrid(Grid grid, int gridSize, CellConfig cellConfig)
        {
            _hexagonGrid = _factory.CreateHexagonGrid(grid);
            _gridRoot = _hexagonGrid.transform;
            
            float inradius = GeometryUtils.InradiusFromOutRadius(cellConfig.CellSize);
            grid.cellSize = new Vector3(inradius, cellConfig.CellSize, 1f);
            grid.cellSwizzle = GridLayout.CellSwizzle.XZY;

            int negativeGridSize = -gridSize;
            
            for (int x = negativeGridSize; x <= gridSize; x++)
            {
                for (int y = negativeGridSize; y <= gridSize; y++)
                    SpawnGridCell(x, y, cellConfig.CellColor, cellConfig.CellHighlightColor, grid, gridSize);
            }
            
            return _hexagonGrid;
        }

        private void SpawnGridCell(int x, int y, Color normalColor, Color highlightColor, Grid grid, int gridSize)
        {
            Vector3 spawnPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
            float maxGridRadius = grid.CellToWorld(Vector3Int.right).magnitude * gridSize;

            if (spawnPosition.magnitude > maxGridRadius) 
                return;

            Vector2Int positionOnGrid = new(x, y);
            
            GridCell gridCell = _factory.CreateGridCell(spawnPosition, positionOnGrid, _gridRoot, normalColor, highlightColor);
            gridCell.gameObject.name = $"({x}, {y})";
                
            _hexagonGrid.AddCell(positionOnGrid, gridCell);
        }
    }
}