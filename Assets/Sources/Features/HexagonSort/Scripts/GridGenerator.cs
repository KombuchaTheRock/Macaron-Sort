using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Scripts
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private float _cellSize;
        [SerializeField] private int _gridSize;

        private IGameFactory _factory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }

        private void Start() => 
            GenerateGrid();

        private void GenerateGrid()
        {
            _grid.cellSize = new Vector3(CalculateInradius(_cellSize), _cellSize, 1f);

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

            _factory.CreateHexagon(spawnPosition, transform);
        }

        private float CalculateInradius(float circumRadius) =>   //Можно вынести в отдельный класс
            circumRadius * Mathf.Cos(Mathf.Deg2Rad * 30f);
      }
}
