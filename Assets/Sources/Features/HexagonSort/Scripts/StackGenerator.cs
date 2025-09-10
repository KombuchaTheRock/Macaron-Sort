using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Scripts
{
    public class StackGenerator : MonoBehaviour
    {
        [SerializeField] private Transform _stackPositionsParent;
        [SerializeField] private float _hexagonHeight; //Config
        [SerializeField] private int _minStackSize; //Config
        [SerializeField] private int _maxStackSize; //Config
        [Space(5), SerializeField] private Color[] _colors; //Config

        private IGameFactory _factory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }

        private void Start() => //Вынести в LoadLevelState
            GenerateStacks();

        private void GenerateStacks()
        {
            for (int i = 0; i < _stackPositionsParent.childCount; i++)
                GenerateStack(_stackPositionsParent.GetChild(i));
        }

        private void GenerateStack(Transform parent)
        {
            GameObject hexagonStack = _factory.CreateHexagonStack(parent.position, parent);
            hexagonStack.name = $"Stack {parent.GetSiblingIndex()}";

            int amount = Random.Range(_minStackSize, _maxStackSize); //Вынести в фабрику
            int firstColorHexagonAmount = Random.Range(0, amount);

            Color[] randomColors = GetRandomColors();

            for (int i = 0; i < amount; i++) 
                SpawnHexagon(i, parent, hexagonStack, firstColorHexagonAmount, randomColors);
        }

        private void SpawnHexagon(int index, Transform parent, GameObject hexagonStack, int firstColorHexagonAmount,
            Color[] randomColors)
        {
            Vector3 hexagonLocalPosition = Vector3.up * index * _hexagonHeight; 
            Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);

            Hexagon hexagon = _factory.CreateHexagon(spawnPosition, parent);
            Color color = index <= firstColorHexagonAmount ? randomColors[0] : randomColors[1];
            
            hexagon.SetColor(color);
        }

        private Color[] GetRandomColors()
        {
            Color firstColor = _colors.OrderBy(_ => Random.value).First();
            Color secondColor = _colors.OrderBy(_ => Random.value).First();

            return new[] { firstColor, secondColor };
        }
    }
}