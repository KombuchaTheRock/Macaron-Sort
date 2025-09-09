using NaughtyAttributes;
using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    public class GridTest : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [OnValueChanged("UpdateGridPosition")]
        [SerializeField] private Vector3Int _gridPosition;

        private void UpdateGridPosition() => 
            transform.position = _grid.CellToWorld(_gridPosition);
    }
}
