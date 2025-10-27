using System;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    [Serializable]
    public class HexagonTileData
    {
        public HexagonTileType TileType;
        public Hexagon HexagonPrefab;
        public int ScoreAmount;
        public Material HexagonMaterial;
    }
}