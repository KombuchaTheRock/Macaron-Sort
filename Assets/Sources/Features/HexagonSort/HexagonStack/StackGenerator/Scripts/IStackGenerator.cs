using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        public void GenerateStacks(Vector3[] spawnPositions, int minStackSize, int maxStackSize, float hexagonHeight,
            Color[] colors, float delayBetweenStacks = 0);
    }
}