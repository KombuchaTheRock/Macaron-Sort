using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;

public class SimpleCellLock : CellLock, IComparable<SimpleCellLock>
{
    private static int _nextId = 1;
    private readonly int _uniqueId;
    
    public event Action<int> StackToUnlockCountChanged;
    
    private int _completedStacksToUnlock;

    public int CompletedStacksToUnlock
    {
        get => _completedStacksToUnlock;
        
        private set
        {
            if (_completedStacksToUnlock == value)
                return;

            _completedStacksToUnlock = value;
            StackToUnlockCountChanged?.Invoke(value);
        }
    }

    public SimpleCellLock(int completedStacksToUnlock)
    {
        _uniqueId = _nextId++;
        CompletedStacksToUnlock = completedStacksToUnlock;
    }

    public override CellLockData ToData() => 
        new SimpleCellLockData(CompletedStacksToUnlock);

    public void DecreaseCompletedStacksToUnlock()
    {
        if (CompletedStacksToUnlock <= 0)
            return;

        CompletedStacksToUnlock--;

        if (CompletedStacksToUnlock <= 0)
            Unlock();
    }
    
    public int CompareTo(SimpleCellLock other)
    {
        if (ReferenceEquals(this, other))
            return 0;

        if (CompletedStacksToUnlock == other.CompletedStacksToUnlock)
            return _uniqueId.CompareTo(other._uniqueId);

        if (CompletedStacksToUnlock < other.CompletedStacksToUnlock)
            return -1;

        return 1;
    }
}