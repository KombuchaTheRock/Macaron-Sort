using System;
using Sources.Features.HexagonSort.GridSystem.GridModificator.Scripts.CellLock;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public class CellLocker : MonoBehaviour
    {
        public event Action<CellLock> Locked;
        public event Action<CellLock> Unlocked;

        private CellLock _currentCellLock;

        public bool IsLocked => _currentCellLock is { IsLocked: true };
        
        public CellLock CurrentCellLock
        {
            get => _currentCellLock;
            private set
            {
                if (Equals(_currentCellLock, value))
                    return;

                if (_currentCellLock != null)
                {
                    _currentCellLock.Locked -= OnCellLocked;
                    _currentCellLock.Unlocked -= OnCellUnlocked;
                }

                _currentCellLock = value;


                if (_currentCellLock != null)
                {
                    _currentCellLock.Locked += OnCellLocked;
                    _currentCellLock.Unlocked += OnCellUnlocked;
                }
            }
        }

        public void SetCellLock(CellLock cellLock) => 
            CurrentCellLock = cellLock;

        public void Lock()
        {
            if (IsLocked || CurrentCellLock == null) 
                return;
            
            CurrentCellLock.Lock();
        }
        
        private void OnCellUnlocked(CellLock obj) => 
            Unlocked?.Invoke(obj);

        private void OnCellLocked(CellLock obj) => 
            Locked?.Invoke(obj);
    }
}