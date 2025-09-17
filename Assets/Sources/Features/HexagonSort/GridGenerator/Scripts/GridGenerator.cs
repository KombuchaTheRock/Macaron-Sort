using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public class GridGenerator : IGridGenerator
    {
        private readonly IGameFactory _factory;
        private Transform _gridRoot;

        public GridGenerator(IGameFactory gameFactory) => 
            _factory = gameFactory;

        public void GenerateGrid(Grid grid, int gridSize, CellConfig cellConfig)
        {
            _gridRoot = _factory.CreateGridRoot();
            
            float inradius = GeometryUtils.InradiusFromOutRadius(cellConfig.CellSize);
            grid.cellSize = new Vector3(inradius, cellConfig.CellSize, 1f);
            grid.cellSwizzle = GridLayout.CellSwizzle.XZY;

            int negativeGridSize = -gridSize;
            
            for (int x = negativeGridSize; x <= gridSize; x++)
            {
                for (int y = negativeGridSize; y <= gridSize; y++)
                    SpawnGridCell(x, y, cellConfig.CellColor, cellConfig.CellHighlightColor, grid, gridSize);
            }
        }

        private void SpawnGridCell(int x, int y, Color normalColor, Color highlightColor, Grid grid, int gridSize)
        {
            Vector3 spawnPosition = grid.CellToWorld(new Vector3Int(x, y, 0));
            float maxGridRadius = grid.CellToWorld(Vector3Int.right).magnitude * gridSize;
            
            if (spawnPosition.magnitude <= maxGridRadius)
                _factory.CreateGridCell(spawnPosition, _gridRoot, normalColor, highlightColor);
        }
    }
}