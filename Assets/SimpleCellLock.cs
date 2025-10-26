using System;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using UnityEngine;

public class SimpleCellLock : CellLock
{
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
        CompletedStacksToUnlock = completedStacksToUnlock;
    }

    public void DecreaseCompletedStacksToUnlock()
    {
        Debug.Log("DecreaseCompletedStacksToUnlock");

        if (CompletedStacksToUnlock <= 0)
            return;

        CompletedStacksToUnlock--;

        if (CompletedStacksToUnlock <= 0)
            Unlock();
    }

    public override CellLockData ToData() => 
        new SimpleCellLockData(CompletedStacksToUnlock);
}