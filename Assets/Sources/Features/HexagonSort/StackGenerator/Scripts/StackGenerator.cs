using System.Collections.Generic;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public class StackGenerator : IStackGenerator, IInitializable
    {
        private float _hexagonHeight;
        private int _minStackSize;
        private int _maxStackSize;
        private Color[] _colors;
        private List<Vector3> _spawnPositions;
        private Transform _stacksRoot;

        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;

        public StackGenerator(IGameFactory gameFactory, IStaticDataService staticData)
        {
            _factory = gameFactory;
            _staticData = staticData;
        }

        public void Initialize()
        {
            HexagonStackConfig stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
            List<Vector3> spawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints;

            _spawnPositions = spawnPositions;
            _hexagonHeight = stackConfig.HexagonHeight;
            _minStackSize = stackConfig.MinStackSize;
            _maxStackSize = stackConfig.MaxStackSize;
            _colors = stackConfig.Colors;
        }

        public void GenerateStacks()
        {
            _stacksRoot = _factory.CreateStacksRoot();

            foreach (Vector3 position in _spawnPositions)
                GenerateStack(position);
        }

        private void GenerateStack(Vector3 spawnPosition)
        {
            HexagonStack hexagonStack = _factory.CreateHexagonStack(spawnPosition, _stacksRoot);
            hexagonStack.name = $"Stack";

            int amount = Random.Range(_minStackSize, _maxStackSize);

            Color[] randomColors = GetRandomColors();
            int firstColorIndex = Random.Range(0, amount);

            for (int i = 0; i < amount; i++)
                SpawnHexagon(i, hexagonStack, randomColors, firstColorIndex);
        }

        private void SpawnHexagon(int index, HexagonStack hexagonStack,
            Color[] randomColors, int firstColorIndex)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * _hexagonHeight;
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            Color color = index <= firstColorIndex ? randomColors[0] : randomColors[1];
            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, hexagonStack.transform, color);

            hexagonStack.Add(hexagon);
            hexagon.SetStack(hexagonStack);
        }

        private Color[] GetRandomColors()
        {
            Color firstColor = _colors[Random.Range(0, _colors.Length)];
            Color secondColor = _colors[Random.Range(0, _colors.Length)];

            return new[] { firstColor, secondColor };
        }
    }
}