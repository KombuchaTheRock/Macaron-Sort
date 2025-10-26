using System;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class SimpleCellLockData : CellLockData
    {
        public int CompletedStacksToUnlock;

        public SimpleCellLockData(int completedStacksToUnlock)
        {
            CompletedStacksToUnlock = completedStacksToUnlock;
        }
    }
}