using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Scripts
{
    public class StackGenerator : MonoBehaviour
    {
        [SerializeField] private Color[] _colors;
        [SerializeField] private float _hexagonHeight;
        [SerializeField] private int _minStackSize;
        [SerializeField] private int _maxStackSize;
        [SerializeField] private Transform _stackPositionsParent;
        
        private IGameFactory _factory;

        [Inject]
        public void Construct(IGameFactory gameFactory)
        {
            _factory = gameFactory;
        }
        
        private void Start() => 
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
            
            int amount = Random.Range(_minStackSize, _maxStackSize);
            int firstColorHexagonAmount = Random.Range(0, amount);

            Color[] randomColors = GetRandomColors();
            
            for (int i = 0; i < amount; i++)
            {
                _hexagonHeight = 0.2f;
                Vector3 hexagonLocalPosition = Vector3.up * i * _hexagonHeight;
                Vector3 spawnPosition = hexagonStack.transform.TransformPoint(hexagonLocalPosition);
                GameObject hexagonInstance = _factory.CreateHexagon(spawnPosition, parent);
                Hexagon hexagon = hexagonInstance.GetComponent<Hexagon>();

                hexagon.Color = i <= firstColorHexagonAmount ? randomColors[0] : randomColors[1];
            }
        }

        private Color[] GetRandomColors()
        {
            List<Color> colors = new List<Color>();
            colors.AddRange(_colors);

            if (colors.Count <= 0)
            {
                Debug.LogError("No color found!");
                return null;
            }
            
            Color firstColor = colors.OrderBy(x => Random.value).First();
            colors.Remove(firstColor);
            
            if (colors.Count <= 0)
            {
                Debug.LogError("Only one color was found!");
                return null;
            }
            
            Color secondColor = colors.OrderBy(x => Random.value).First();
            
            return new[] { firstColor, secondColor };
        }
    }
}
