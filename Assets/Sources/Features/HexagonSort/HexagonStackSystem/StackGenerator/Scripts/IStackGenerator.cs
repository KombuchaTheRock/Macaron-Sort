using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        public void GenerateStacks(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks = 0);
    }
}