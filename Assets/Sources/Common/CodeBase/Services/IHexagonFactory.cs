using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IHexagonFactory
    {
        List<HexagonStack> Stacks { get; }
        Transform CreateStacksRoot();
        Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles);
    }
}