using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        public void GenerateStacks(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks = 0, Action<List<HexagonStack>> onStacksGenerated = null);

        HexagonStack GenerateStack(Vector3 spawnPosition, HexagonStackConfig stackConfig, HexagonTileType[] hexagons = null);
    }
}