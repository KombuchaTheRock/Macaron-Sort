using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public abstract class GeneratedStack
    {
        [field: SerializeField] public HexagonTileType[] Tiles { get; private set; }

        protected GeneratedStack(HexagonTileType[] tiles) =>
            Tiles = tiles;
    }
}