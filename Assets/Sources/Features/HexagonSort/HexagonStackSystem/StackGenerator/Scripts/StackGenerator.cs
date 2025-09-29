using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts
{
    public class StackGenerator : IStackGenerator
    {
        private const string HexagonStackName = "Stack";

        private readonly IHexagonFactory _hexagonFactory;
        private readonly ICoroutineRunner _coroutineRunner;

        private Transform _stacksRoot;
        private Coroutine _stackGenerateRoutine;

        public StackGenerator(IHexagonFactory hexagonHexagonFactory, ICoroutineRunner coroutineRunner)
        {
            _hexagonFactory = hexagonHexagonFactory;
            _coroutineRunner = coroutineRunner;
        }

        public void GenerateStacks(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks = 0, Action<List<HexagonStack>> onStacksGenerated = null)
        {
            _stacksRoot ??= _hexagonFactory.CreateStacksRoot();

            if (_stackGenerateRoutine != null)
                _coroutineRunner.StopCoroutine(_stackGenerateRoutine);

            _stackGenerateRoutine = _coroutineRunner.StartCoroutine(GenerateStacksRoutine(spawnPositions,
                stackConfig,
                delayBetweenStacks,
                onStacksGenerated));
        }

        public HexagonStack GenerateStack(Vector3 spawnPosition, HexagonStackConfig stackConfig,
            HexagonTileType[] hexagons = null)
        {
            HexagonStack hexagonStack =
                _hexagonFactory.CreateHexagonStack(spawnPosition, _stacksRoot, stackConfig.OffsetBetweenTiles);
            hexagonStack.name = HexagonStackName;

            HexagonTileType[] tiles;
            int amount;

            if (hexagons == null)
            {
                hexagonStack.ActivateSpawnAnimation();

                amount = Random.Range(stackConfig.MinStackSize, stackConfig.MaxStackSize + 1);
                tiles = TileShuffler.GetRandomTileTypes(amount, stackConfig.MaxTileChanges);
            }
            else
            {
                amount = hexagons.Length;
                tiles = hexagons;
            }

            for (int i = 0; i < amount; i++)
                SpawnHexagon(i, hexagonStack, tiles, stackConfig.OffsetBetweenTiles);

            return hexagonStack;
        }

        private IEnumerator GenerateStacksRoutine(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks, Action<List<HexagonStack>> onStacksGenerated = null)
        {
            List<HexagonStack> generatedStacks = new();

            foreach (Vector3 position in spawnPositions)
            {
                HexagonStack stack = GenerateStack(position, stackConfig);
                generatedStacks.Add(stack);

                yield return new WaitForSeconds(delayBetweenStacks);
            }

            onStacksGenerated?.Invoke(generatedStacks);
        }

        private void SpawnHexagon(int index, HexagonStack hexagonStack,
            HexagonTileType[] randomTiles, float offsetBetweenTiles)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * offsetBetweenTiles;
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            Hexagon hexagon = _hexagonFactory.CreateHexagon(spawnPosition, randomTiles[index], hexagonStack.transform);

            hexagonStack.Add(hexagon);
        }
    }
}