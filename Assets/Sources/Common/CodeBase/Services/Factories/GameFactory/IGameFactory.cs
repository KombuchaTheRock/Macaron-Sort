using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Factories.GameFactory
{
    public interface IGameFactory
    {
        void CreateInstanceRoot();
        GridCell CreateGridCell(Vector3 position, Vector2Int positionOnGrid, Transform parent, Color normalColor, Color highlightColor);
        List<GridCell> GridCells { get; }
        List<IProgressReader> ProgressReaders { get; }
        HexagonGrid CreateHexagonGrid();
        GameObject CreateHUD();
    }
}