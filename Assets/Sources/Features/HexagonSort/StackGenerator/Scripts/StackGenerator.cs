using System.Collections.Generic;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Sources.Features.HexagonSort.Scripts
{
    public class StackGenerator : MonoBehaviour
    {
        private float _hexagonHeight;
        private int _minStackSize;
        private int _maxStackSize;
        private Color[] _colors;
        private List<Vector3> _spawnPositions;

        private IGameFactory _factory;

        [Inject]
        public void Construct(IGameFactory gameFactory) =>
            _factory = gameFactory;

        public void Initialize(HexagonStackConfig stackConfig, List<Vector3> spawnPositions)
        {
            _spawnPositions = spawnPositions;
            _hexagonHeight = stackConfig.HexagonHeight;
            _minStackSize = stackConfig.MinStackSize;
            _maxStackSize = stackConfig.MaxStackSize;
            _colors = stackConfig.Colors;
        }

        public void GenerateStacks()
        {
            foreach (Vector3 position in _spawnPositions)
                GenerateStack(position);
        }

        private void GenerateStack(Vector3 spawnPosition)
        {
            HexagonStack hexagonStack = _factory.CreateHexagonStack(spawnPosition, transform);
            hexagonStack.name = $"Stack {transform.GetSiblingIndex()}";

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

            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, hexagonStack.transform);
            
            Color color = index <= firstColorIndex ? randomColors[0] : randomColors[1];
            hexagon.GetComponent<MeshColorSwitcher>().SetColor(color);
            
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