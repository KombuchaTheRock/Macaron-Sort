using System.Collections.Generic;
using Sources.Common.CodeBase.Services.Settings;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Factories.HexagonFactory
{
    public interface IHexagonFactory
    {
        List<ISettingsReader> SettingsReaders { get; }
        List<HexagonStack> Stacks { get; }
        Transform CreateStacksRoot();
        Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent);
        HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles);
    }
}