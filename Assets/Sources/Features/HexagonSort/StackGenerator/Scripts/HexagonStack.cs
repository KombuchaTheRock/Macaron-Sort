using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackGenerator.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        private List<Hexagon> _hexagons = new();

        public Hexagon FirstHexagon => _hexagons[^1];
        
        public void Add(Hexagon hexagon)
        {
            _hexagons.Add(hexagon);
        }
    }
}