using System.Linq;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class GridCellBlocker
    {
        private HexagonGrid _hexagonGrid;
        
        public GridCellBlocker(HexagonGrid hexagonGrid) => 
            _hexagonGrid = hexagonGrid;
        
        public void BlockRandomFreeCell()
        {
            GridCell[] freeCells = _hexagonGrid.Cells.Where(cell => cell.IsOccupied == false).ToArray();
            GridCell randomCell = freeCells[Random.Range(0, freeCells.Length - 1)];

            randomCell.Block();
        }
    }
}