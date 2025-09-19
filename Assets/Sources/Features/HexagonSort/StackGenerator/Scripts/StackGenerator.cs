using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public class StackGenerator : IStackGenerator
    {
        private const string HexagonStackName = "Stack";
        
        private Transform _stacksRoot;
        private float _currentStackHeight;

        private readonly IGameFactory _factory;

        public StackGenerator(IGameFactory gameFactory) => 
            _factory = gameFactory;

        public void GenerateStacks(Vector3[] spawnPositions, int minStackSize, int maxStackSize, float hexagonHeight,
            Color[] colors)
        {
            _stacksRoot = _factory.CreateStacksRoot();

            foreach (Vector3 position in spawnPositions)
                GenerateStack(position, minStackSize, maxStackSize, hexagonHeight, colors);
        }

        private void GenerateStack(Vector3 spawnPosition, int minStackSize, int maxStackSize, float hexagonHeight,
            Color[] colors)
        {
            HexagonStack hexagonStack = _factory.CreateHexagonStack(spawnPosition, _stacksRoot);
            hexagonStack.name = HexagonStackName;

            int amount = Random.Range(minStackSize, maxStackSize);

            Color[] randomColors = GetRandomColors(colors);
            int firstColorIndex = Random.Range(0, amount);

            for (int i = 0; i <= amount; i++)
                SpawnHexagon(i, hexagonStack, randomColors, firstColorIndex, hexagonHeight);

            SetStackColliderHeight(hexagonStack, amount, hexagonHeight);
        }

        private void SpawnHexagon(int index, HexagonStack hexagonStack,
            Color[] randomColors, int firstColorIndex, float hexagonHeight)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * hexagonHeight;
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            Color color = index <= firstColorIndex ? randomColors[0] : randomColors[1];
            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, hexagonStack.transform, color);

            hexagonStack.Add(hexagon);
        }

        private void SetStackColliderHeight(HexagonStack hexagonStack, int amount, float hexagonHeight)
        {
            HexagonStackCollider hexagonStackCollider = hexagonStack.GetComponent<HexagonStackCollider>();

            float stackHeight = (amount + 1) * hexagonHeight;
            float stackColliderHeightMultiplier = stackHeight / hexagonStackCollider.OriginalHeight;

            hexagonStackCollider.SetHeight(stackColliderHeightMultiplier);
        }

        private Color[] GetRandomColors(Color[] colors)
        {
            Color firstColor = colors[Random.Range(0, colors.Length)];
            Color secondColor = colors[Random.Range(0, colors.Length)];

            return new[] { firstColor, secondColor };
        }
    }
}