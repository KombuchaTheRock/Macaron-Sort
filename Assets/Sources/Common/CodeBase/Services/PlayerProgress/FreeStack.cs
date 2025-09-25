using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class FreeStack : GeneratedStack
    {
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; }

        public FreeStack(HexagonTileType[] tiles, Vector3 spawnPosition) : base(tiles) => 
            SpawnPosition = spawnPosition;
    }
}