using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class CellData
    {
        [field: SerializeField] public Vector2Int PositionOnGrid { get; private set; }
        [field: SerializeField] public SimpleCellLockData SimpleLockData { get; private set; }
        [field: SerializeField] public TileScoreCellLockData TileScoreLockData { get; private set; }
        [field: SerializeField] public bool IsLocked { get; private set; }
        [field: SerializeField] public CellLockType LockType { get; private set; }

        public CellData(GridCell gridCell)
        {
            PositionOnGrid = gridCell.PositionOnGrid;
            IsLocked = gridCell.IsLocked;

            if (IsLocked)
                InitializeLockData(gridCell);
        }

        private void InitializeLockData(GridCell gridCell)
        {
            CellLockData lockData = gridCell.Locker.CurrentCellLock.ToData();

            switch (lockData)
            {
                case SimpleCellLockData simpleCellLockData:
                    SimpleLockData = simpleCellLockData;
                    LockType = CellLockType.Simple;
                    Debug.Log(simpleCellLockData.CompletedStacksToUnlock);
                    break;
                case TileScoreCellLockData tileScoreLockData:
                    TileScoreLockData = tileScoreLockData;
                    LockType = CellLockType.TileScore;
                    break;
            }
        }
    }
}