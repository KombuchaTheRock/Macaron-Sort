using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

public class TileScoreCellLock : CellLock
{
    public event Action<int> ScoreToUnlockChanged;
    
    public HexagonTileType TileType { get; }
    public int ScoreToUnlock { get; private set; }

    public override CellLockData ToData() => 
        new TileScoreCellLockData();

    public TileScoreCellLock(HexagonTileType tileType, int scoreToUnlock)
    {
        TileType = tileType;
        ScoreToUnlock = scoreToUnlock;
    }

    public void DecreaseScoreToUnlock(int amount)
    {
        if (amount <= 0)
            return;

        ScoreToUnlock -= Math.Min(amount, ScoreToUnlock);

        if (ScoreToUnlock <= 0)
            Unlock();
        
        ScoreToUnlockChanged?.Invoke(ScoreToUnlock);
    }
}