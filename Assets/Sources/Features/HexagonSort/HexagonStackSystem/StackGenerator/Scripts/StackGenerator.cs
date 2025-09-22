using System.Collections;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Infrastructure.Extensions;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public class StackGenerator : IStackGenerator
    {
        private const string HexagonStackName = "Stack";

        private Transform _stacksRoot;
        private float _currentStackHeight;

        private readonly IGameFactory _factory;
        private readonly ICoroutineRunner _coroutineRunner;
        private Coroutine _stackGenerateRoutine;

        public StackGenerator(IGameFactory gameFactory, ICoroutineRunner coroutineRunner)
        {
            _factory = gameFactory;
            _coroutineRunner = coroutineRunner;
        }

        public void GenerateStacks(Vector3[] spawnPositions, int minStackSize, int maxStackSize, float verticalOffsetBetweenTiles, float delayBetweenStacks = 0)
        {
            _stacksRoot ??= _factory.CreateStacksRoot();

            if (_stackGenerateRoutine != null) 
                _coroutineRunner.StopCoroutine(_stackGenerateRoutine);
            
            _stackGenerateRoutine = _coroutineRunner.StartCoroutine(GenerateStacksRoutine(spawnPositions, minStackSize, maxStackSize,
                verticalOffsetBetweenTiles, delayBetweenStacks));
        }

        private IEnumerator GenerateStacksRoutine(Vector3[] spawnPositions, int minStackSize, int maxStackSize,
            float offsetBetweenTiles, float delayBetweenStacks)
        {
            foreach (Vector3 position in spawnPositions)
            {
                GenerateStack(position, minStackSize, maxStackSize, offsetBetweenTiles);
                yield return new WaitForSeconds(delayBetweenStacks);
            }
        }

        private void GenerateStack(Vector3 spawnPosition, int minStackSize, int maxStackSize, float offsetBetweenTiles)
        {
            HexagonStack hexagonStack = _factory.CreateHexagonStack(spawnPosition, _stacksRoot, offsetBetweenTiles);
            hexagonStack.name = HexagonStackName;
            
            int amount = Random.Range(minStackSize, maxStackSize);

            HexagonTileType[] randomTiles = GetRandomTiles();
            int firstTileIndex = Random.Range(0, amount);

            for (int i = 0; i < amount; i++)
                SpawnHexagon(i, hexagonStack, randomTiles, firstTileIndex, offsetBetweenTiles);
        }

        private void SpawnHexagon(int index, HexagonStack hexagonStack,
            HexagonTileType[] randomTiles, int firstTileIndex, float offsetBetweenTiles)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * offsetBetweenTiles;
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            HexagonTileType tileType = index <= firstTileIndex ? randomTiles[0] : randomTiles[1];
            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, tileType, hexagonStack.transform);

            hexagonStack.Add(hexagon);
        }

        private HexagonTileType[] GetRandomTiles()
        {
            HexagonTileType firstType = EnumExtensions.GetRandomValue<HexagonTileType>();
            HexagonTileType secondType = EnumExtensions.GetRandomValue<HexagonTileType>();
            
            return new[] { firstType, secondType };
        }
    }
}