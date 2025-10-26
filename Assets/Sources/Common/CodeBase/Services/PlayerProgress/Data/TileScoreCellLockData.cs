using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class TileScoreCellLockData : CellLockData
    {
        public int ScoreToUnlock;
        public HexagonTileType TileType;
    }
}