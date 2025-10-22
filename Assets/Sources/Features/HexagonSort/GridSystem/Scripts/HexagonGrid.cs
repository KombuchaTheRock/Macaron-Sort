using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class HexagonGrid : MonoBehaviour
    {
        private Dictionary<Vector2Int, GridCell> _hexagonGrid = new();

        public Grid GridComponent { get; private set; }
        public List<GridCell> Cells => _hexagonGrid.Select(x => x.Value).ToList();

        public void Initialize(Grid grid) =>
            GridComponent = grid;

        public void AddCell(Vector2Int cell, GridCell gridCell) =>
            _hexagonGrid.Add(cell, gridCell);

        public void RemoveCell(GridCell gridCell) =>
            _hexagonGrid.Remove(gridCell.PositionOnGrid);
        
        public bool IsCellOnGrid(Vector2Int cell) =>
            _hexagonGrid.ContainsKey(cell);

        public bool TryGetCell(Vector2Int positionOnGrid, out GridCell gridCell) =>
            _hexagonGrid.TryGetValue(positionOnGrid, out gridCell);
    }
}