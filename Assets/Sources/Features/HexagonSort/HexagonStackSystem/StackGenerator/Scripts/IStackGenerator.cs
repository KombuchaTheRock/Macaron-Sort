using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        public void GenerateStacks(Vector3[] spawnPositions, int minStackSize, int maxStackSize, float hexagonHeight, float delayBetweenStacks = 0);
    }
}