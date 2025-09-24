using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class PlacedStack
    {
        public HexagonTileType[] Tiles;
        public Vector2Int PositionOnGrid;

        public PlacedStack(HexagonTileType[] tiles, Vector2Int positionOnGrid)
        {
            Tiles = tiles;
            PositionOnGrid = positionOnGrid;
        }
    }
}