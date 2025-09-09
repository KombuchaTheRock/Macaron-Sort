using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private float _cellSize;
        [SerializeField] private GameObject _hexagonPrefab;
        [SerializeField] private Grid _grid;

        [OnValueChanged("GenerateGrid")]
        [SerializeField] private int _gridSize;
        
        private void SetGridCellSize() => 
            _grid.cellSize = new Vector3(CalculateInradius(_cellSize), _cellSize, 1f);

        private static float CalculateInradius(float circumRadius) => 
            circumRadius * Mathf.Cos(Mathf.Deg2Rad * 30f);

        private void GenerateGrid()
        {
            SetGridCellSize();
            ClearGrid();

            for (int x = -_gridSize; x <= _gridSize; x++)
            {
                for (int y = -_gridSize; y <= _gridSize; y++)
                {
                    Vector3 spawnPosition = _grid.CellToWorld(new Vector3Int(x, y, 0));

                    float maxGridRadius = _grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * _gridSize;
                    if (spawnPosition.magnitude > maxGridRadius)
                        continue;
                    
                    Instantiate(_hexagonPrefab, spawnPosition, Quaternion.identity, transform);
                }
            }
        }
        
        private void ClearGrid()
        {
            while (transform.childCount > 0)
            {
                Transform child = transform.GetChild(0);
                child.SetParent(null);
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
