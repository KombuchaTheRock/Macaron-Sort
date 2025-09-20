using System.Collections.Generic;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles);
        void CreateInstanceRoot();
        GridCell CreateGridCell(Vector3 position, Vector2Int positionOnGrid, Transform parent, Color normalColor, Color highlightColor);
        List<HexagonStack> Stacks { get; }
        StackMover StackMover { get; }
        GridRotator GridRotator { get; }
        List<GridCell> GridCells { get; }
        HexagonGrid CreateHexagonGrid(Grid grid);
        Transform CreateStacksRoot();
        StackMover CreateStackMover();
        MergeSystem CreateMergeSystem(StackMover stackMover, HexagonGrid hexagonGrid);
    }
}