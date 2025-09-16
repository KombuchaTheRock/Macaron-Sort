using System.Collections.Generic;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IGameFactory
    {
        Hexagon CreateHexagon(Vector3 position, Transform parent, Color color);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent);
        void CreateInstanceRoot();
        GridCell CreateGridCell(Vector3 position, Transform parent, Color normalColor, Color highlightColor);
        StackGenerator StackGenerator { get; }
        List<HexagonStack> Stacks { get; }
        Transform CreateGridRoot();
        Transform CreateStacksRoot();
    }
}