using System.Collections.Generic;
using Sources.Features.HexagonSort.Grid.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Grid.Scripts;
using Sources.Features.HexagonSort.HexagonStack.HexagonTile.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        Hexagon CreateHexagon(Vector3 position, Transform parent, Color color);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent);
        void CreateInstanceRoot();
        GridCell CreateGridCell(Vector3 position, Transform parent, Color normalColor, Color highlightColor);
        List<HexagonStack> Stacks { get; }
        StackMover StackMover { get; }
        GridRotator GridRotator { get; }
        List<GridCell> GridCells { get; }
        Transform CreateGridRoot();
        Transform CreateStacksRoot();
        StackMover CreateStackMover();
    }
}