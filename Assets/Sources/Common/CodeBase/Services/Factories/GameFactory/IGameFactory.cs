using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Factories.GameFactory
{
    public interface IGameFactory
    {
        void CreateInstanceRoot();
        GridCell CreateGridCell(Vector3 position, Vector2Int positionOnGrid, Transform parent, Color normalColor, Color highlightColor);
        StackMover StackMover { get; }
        GridRotator GridRotator { get; }
        List<GridCell> GridCells { get; }
        MergeSystem MergeSystem { get; }
        List<IProgressReader> ProgressReaders { get; }
        HexagonGrid CreateHexagonGrid(Grid grid);
        StackMover CreateStackMover();
        MergeSystem CreateMergeSystem(StackMover stackMover, HexagonGrid hexagonGrid);
        GameObject CreateHUD();
    }
}