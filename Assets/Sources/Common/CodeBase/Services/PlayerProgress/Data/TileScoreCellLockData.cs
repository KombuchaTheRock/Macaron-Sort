using System;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class TileScoreCellLockData : CellLockData
    {
        public int ScoreToUnlock;
        public HexagonTileType TileType;

        public TileScoreCellLockData(HexagonTileType tileType, int scoreToUnlock)
        {
            ScoreToUnlock = scoreToUnlock;
            TileType = tileType;
        }
    }
}