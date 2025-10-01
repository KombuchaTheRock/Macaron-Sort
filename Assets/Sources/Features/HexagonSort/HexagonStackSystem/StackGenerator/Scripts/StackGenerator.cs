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

        private Transform _stacksRoot;
        private Coroutine _stackGenerateRoutine;

        private readonly IHexagonFactory _hexagonFactory;

        public StackGenerator(IHexagonFactory hexagonHexagonFactory) =>
            _hexagonFactory = hexagonHexagonFactory;

        public HexagonStack GenerateStack(Vector3 spawnPosition, HexagonStackConfig stackConfig,
            HexagonTileType[] hexagons = null)
        {
            HexagonStack hexagonStack =
                _hexagonFactory.CreateHexagonStack(spawnPosition, _stacksRoot, stackConfig.OffsetBetweenTiles);
            hexagonStack.name = HexagonStackName;

            (HexagonTileType[] tiles, int amount) = 
                GetStackConfiguration(hexagonStack, stackConfig, hexagons);

            for (int i = 0; i < amount; i++)
                SpawnHexagon(i, hexagonStack, tiles, stackConfig.OffsetBetweenTiles);

            return hexagonStack;
        }

        private (HexagonTileType[] tiles, int amount) GetStackConfiguration(
            HexagonStack hexagonStack,
            HexagonStackConfig stackConfig,
            HexagonTileType[] hexagons)
        {
            if (hexagons != null)
                return (hexagons, hexagons.Length);

            hexagonStack.ActivateSpawnAnimation();
            
            int amount = Random.Range(stackConfig.MinStackSize, stackConfig.MaxStackSize + 1);
            HexagonTileType[] tiles = TileShuffler.GetRandomTileTypes(amount, stackConfig.MaxTileChanges);

            return (tiles, amount);
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