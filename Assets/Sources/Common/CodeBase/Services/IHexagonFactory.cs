using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IHexagonFactory
    {
        Transform CreateStacksRoot();
        List<HexagonStack> Stacks { get; }
        Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles);
    }
}