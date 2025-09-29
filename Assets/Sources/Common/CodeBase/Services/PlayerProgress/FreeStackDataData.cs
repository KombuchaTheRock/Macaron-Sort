using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class FreeStackDataData : GeneratedStackData
    {
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; }

        public FreeStackDataData(HexagonTileType[] tiles, Vector3 spawnPosition) : base(tiles) => 
            SpawnPosition = spawnPosition;
    }
}