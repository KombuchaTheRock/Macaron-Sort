using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

public class TileScoreCellLock : CellLock, IComparable<TileScoreCellLock>
{
    private static int _nextId = 1;
    private readonly int _uniqueId;
    
    public event Action<int> ScoreToUnlockChanged;

    public HexagonTileType TileType { get; }
    public int ScoreToUnlock { get; private set; }

    public TileScoreCellLock(HexagonTileType tileType, int scoreToUnlock)
    {
        _uniqueId = _nextId++;
        TileType = tileType;
        ScoreToUnlock = scoreToUnlock;
    }

    public override CellLockData ToData() =>
        new TileScoreCellLockData(TileType, ScoreToUnlock);

    public void DecreaseScoreToUnlock(int amount)
    {
        if (amount <= 0)
            return;

        ScoreToUnlock -= Math.Min(amount, ScoreToUnlock);

        if (ScoreToUnlock <= 0)
            Unlock();

        ScoreToUnlockChanged?.Invoke(ScoreToUnlock);
    }

    public int CompareTo(TileScoreCellLock other)
    {
        if (ReferenceEquals(this, other))
            return 0;

        if (ScoreToUnlock == other.ScoreToUnlock)
            return _uniqueId.CompareTo(other._uniqueId);
        
        if (ScoreToUnlock < other.ScoreToUnlock)
            return -1;
        
        return 1;
    }
}