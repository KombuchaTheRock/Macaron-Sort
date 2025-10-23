using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class GridData
    {
        [field: SerializeField] public List<CellData> Cells { get; private set; }

        public GridData(List<CellData> cells) => 
            Cells = cells;

        public void UpdateGridData(HexagonGrid hexagonGrid)
        {
            Cells = hexagonGrid.Cells.Select(cell => new CellData(cell.PositionOnGrid)).ToList();
        }
    }
}