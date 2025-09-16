using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackMover.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        [SerializeField] private StackMovement _movement;
        
        private List<Hexagon> _hexagons = new();
        public Hexagon FirstHexagon => _hexagons[^1];
        public StackMovement Movement => _movement;
        
        public void Add(Hexagon hexagon) => 
            _hexagons.Add(hexagon);
    }
}