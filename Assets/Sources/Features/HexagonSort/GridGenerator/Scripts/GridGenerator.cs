using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public class GridGenerator : IGridGenerator, IInitializable
    {
        private Grid _grid;
        private Color _normalCellColor;
        private Color _highlightCellColor;
        private int _gridSize;
        private float _cellSize;
        private Transform _gridRoot;

        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;

        public GridGenerator(IGameFactory gameFactory, IStaticDataService staticData)
        {
            _factory = gameFactory;
            _staticData = staticData;
        }

        public void Initialize()
        {
            GridConfig gridConfig = _staticData.GameConfig.GridConfig;
            
            _normalCellColor = gridConfig.CellColor;
            _grid = gridConfig.Grid;
            _cellSize = gridConfig.CellSize;
            _gridSize = gridConfig.GridRadius;
            _highlightCellColor = gridConfig.CellHighlightColor;
        }

        public void GenerateGrid()
        {
            _gridRoot = _factory.CreateGridRoot();
            
            float inradius = GeometryUtils.InradiusFromOutRadius(_cellSize);
            _grid.cellSize = new Vector3(inradius, _cellSize, 1f);
            _grid.cellSwizzle = GridLayout.CellSwizzle.XZY;

            int negativeGridSize = -_gridSize;
            
            for (int x = negativeGridSize; x <= _gridSize; x++)
            {
                for (int y = negativeGridSize; y <= _gridSize; y++)
                    SpawnGridCell(x, y);
            }
        }

        private void SpawnGridCell(int x, int y)
        {
            Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(x, y, 0));
            float maxGridRadius = _grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * _gridSize;

            if (spawnPosition.magnitude <= maxGridRadius)
                _factory.CreateGridCell(spawnPosition, _gridRoot, _normalCellColor, _highlightCellColor);
        }
    }
}