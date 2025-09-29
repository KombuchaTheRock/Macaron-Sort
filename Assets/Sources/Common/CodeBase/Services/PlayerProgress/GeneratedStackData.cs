using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public abstract class GeneratedStackData
    {
        [field: SerializeField] public HexagonTileType[] Tiles { get; private set; }

        protected GeneratedStackData(HexagonTileType[] tiles) =>
            Tiles = tiles;
    }
}