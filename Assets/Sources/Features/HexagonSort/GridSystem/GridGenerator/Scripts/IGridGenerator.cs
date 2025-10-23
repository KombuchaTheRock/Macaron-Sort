using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public interface IGridGenerator
    {
        public HexagonGrid GenerateNewGrid(Grid grid, int gridSize, CellConfig cellConfig);
        GridCell GenerateGridCell(Vector2Int positionOnGrid, Vector3 worldPosition, CellConfig cellConfig);
        HexagonGrid GenerateSavedGrid(Grid grid, CellConfig cellConfig, List<CellData> cellData);
    }
}