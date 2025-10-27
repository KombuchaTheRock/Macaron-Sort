using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public interface IGridGenerator
    {
        public HexagonGrid GenerateNewGrid(int gridSize, CellConfig cellConfig);
        GridCell GenerateGridCell(Vector2Int positionOnGrid, CellConfig cellConfig);
        HexagonGrid GenerateSavedGrid(CellConfig cellConfig, List<CellData> cellData);
    }
}