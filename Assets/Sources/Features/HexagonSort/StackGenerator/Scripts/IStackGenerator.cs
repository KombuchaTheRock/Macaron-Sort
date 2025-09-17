using UnityEngine;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public interface IStackGenerator
    {
        public void GenerateStacks(Vector3[] spawnPositions, int minStackSize, int maxStackSize, float hexagonHeight,
            Color[] colors);
    }
}