using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellsDeleter
    {
        private HexagonGrid _hexagonGrid;

        public GridCellsDeleter(HexagonGrid hexagonGrid) => 
            _hexagonGrid = hexagonGrid;

        public void DeleteRandomFreeCell()
        {
            GridCell[] freeCells = _hexagonGrid.Cells.Where(cell => cell.IsOccupied == false).ToArray();
            GridCell randomCell = freeCells[Random.Range(0, freeCells.Length - 1)];

            _hexagonGrid.RemoveCell(randomCell);
            Object.Destroy(randomCell.gameObject);
        }
    }
}