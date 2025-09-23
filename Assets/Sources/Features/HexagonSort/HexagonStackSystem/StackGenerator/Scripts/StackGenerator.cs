using System.Collections;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services;
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

        public void GenerateStacks(Vector3[] spawnPositions, HexagonStackConfig stackConfig, float delayBetweenStacks = 0)
        {
            _stacksRoot ??= _factory.CreateStacksRoot();

            if (_stackGenerateRoutine != null)
                _coroutineRunner.StopCoroutine(_stackGenerateRoutine);

            _stackGenerateRoutine = _coroutineRunner.StartCoroutine(GenerateStacksRoutine(spawnPositions,
               stackConfig,
                delayBetweenStacks));
        }

        private IEnumerator GenerateStacksRoutine(Vector3[] spawnPositions, HexagonStackConfig stackConfig, float delayBetweenStacks)
        {
            foreach (Vector3 position in spawnPositions)
            {
                GenerateStack(position, stackConfig);
                yield return new WaitForSeconds(delayBetweenStacks);
            }
        }

        private void GenerateStack(Vector3 spawnPosition, HexagonStackConfig stackConfig)
        {
            HexagonStack hexagonStack = _factory.CreateHexagonStack(spawnPosition, _stacksRoot, stackConfig.OffsetBetweenTiles);
            hexagonStack.name = HexagonStackName;

            int amount = Random.Range(stackConfig.MinStackSize, stackConfig.MaxStackSize + 1);
            HexagonTileType[] randomTiles = TileShuffler.GetRandomTileTypes(amount, 3);

            for (int i = 0; i < amount; i++)
                SpawnHexagon(i, hexagonStack, randomTiles, stackConfig.OffsetBetweenTiles);
            
            hexagonStack.UpdateStackSizeDisplay();
        }

        private void SpawnHexagon(int index, HexagonStack hexagonStack,
            HexagonTileType[] randomTiles, float offsetBetweenTiles)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * offsetBetweenTiles;
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, randomTiles[index], hexagonStack.transform);

            hexagonStack.Add(hexagon);
        }
    }
}