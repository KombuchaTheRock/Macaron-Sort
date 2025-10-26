using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;

public abstract class CellLock
{
    public event Action<CellLock> Locked;
    public event Action<CellLock> Unlocked;
    
    public bool IsLocked { get; private set; }

    public abstract CellLockData ToData();
    
    public static CellLock FromData(CellLockData data)
    {
        return data switch
        {
            SimpleCellLockData simpleData => new SimpleCellLock(simpleData.CompletedStacksToUnlock),
            TileScoreCellLockData scoreData => new TileScoreCellLock(scoreData.TileType, scoreData.ScoreToUnlock),
            _ => throw new ArgumentException($"Unknown data type: {data.GetType().Name}")
        };
    }
    
    public void Lock()
    {
        IsLocked = true;
        Locked?.Invoke(this);        
    }

    protected void Unlock()
    {
        IsLocked = false;
        Unlocked?.Invoke(this);
    }
}