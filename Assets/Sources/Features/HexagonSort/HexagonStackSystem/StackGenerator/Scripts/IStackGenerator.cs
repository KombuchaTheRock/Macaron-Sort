using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        HexagonStack GenerateStack(Vector3 spawnPosition, HexagonStackConfig stackConfig, HexagonTileType[] hexagons = null);
    }
}