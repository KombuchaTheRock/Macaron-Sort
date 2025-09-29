using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class PlacedStackData : GeneratedStackData
    {
        [field: SerializeField] public Vector2Int PositionOnGrid { get; private set; }

        public PlacedStackData(HexagonTileType[] tiles, Vector2Int positionOnGrid) : base(tiles) =>
            PositionOnGrid = positionOnGrid;
    }
}