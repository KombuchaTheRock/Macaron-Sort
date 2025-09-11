using Sources.Common.CodeBase.Infrastructure.Utilities;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public class GridGenerator : MonoBehaviour
    {
        private Grid _grid;
        private Color _cellColor;
        private int _gridSize;
        private float _cellSize;
        
        private IGameFactory _factory;

        [Inject]
        public void Construct(IGameFactory gameFactory) => 
            _factory = gameFactory;

        public void Initialize(GridConfig gridConfig)
        {
            _cellColor = gridConfig.GridColor;
            _grid = gridConfig.Grid;
            _cellSize = gridConfig.CellSize;
            _gridSize = gridConfig.GridRadius;
        }
        
        public void GenerateGrid()
        {
            float inradius = GeometryUtils.InradiusFromCircumradius(_cellSize);
            _grid.cellSize = new Vector3(inradius, _cellSize, 1f);
            _grid.cellSwizzle = GridLayout.CellSwizzle.XZY;
            
            for (int x = -_gridSize; x <= _gridSize; x++)
            {
                for (int y = -_gridSize; y <= _gridSize; y++) 
                    SpawnHexagon(x, y);
            }
        }

        private void SpawnHexagon(int x, int y)
        {
            Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(x, y, 0));
            float maxGridRadius = _grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * _gridSize;
                    
            if (spawnPosition.magnitude > maxGridRadius)
                return;

            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, transform);
            hexagon.SetColor(_cellColor);
        }
      }
}
