using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class HexagonGrid : MonoBehaviour
    {
        private Dictionary<Vector2Int, GridCell> _hexagonGrid = new();
        private Grid _grid;

        public void Initialize(Grid grid) => 
            _grid = grid;

        public void AddCell(Vector2Int cell, GridCell gridCell) =>
            _hexagonGrid.Add(cell, gridCell);

        public bool TryGetCell(Vector2Int positionOnGrid, out GridCell gridCell) => 
            _hexagonGrid.TryGetValue(positionOnGrid, out gridCell);
    }
}