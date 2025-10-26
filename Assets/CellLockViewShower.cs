using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

public class CellLockViewShower : MonoBehaviour
{
    [SerializeField] private CellLocker _cellLocker;
    [SerializeField] private SimpleCellLockView _simpleCellLockView;
    [SerializeField] private TileScoreCellLockView _tileScoreCellLockView;
    
    private void Awake() => 
        SubscribeUpdates();

    private void OnDestroy() => 
        CleanUp();

    private void OnCellLocked(CellLock obj)
    {
        switch (obj)
        {
            case SimpleCellLock simpleCellLock:
                _simpleCellLockView.Initialize(simpleCellLock);
                _simpleCellLockView.Show();
                break;
            case TileScoreCellLock tileScoreCellLock:
                _tileScoreCellLockView.Initialize(tileScoreCellLock);
                _tileScoreCellLockView.Show();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj));
        }
    }

    private void OnCellUnlocked(CellLock obj)
    {
        switch (obj)
        {
            case SimpleCellLock simpleCellLock:
                _simpleCellLockView.Hide();
                break;
            case TileScoreCellLock tileScoreCellLock:
                _tileScoreCellLockView.Hide();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj));
        }
    }

    private void SubscribeUpdates()
    {
        _cellLocker.Locked += OnCellLocked;
        _cellLocker.Unlocked += OnCellUnlocked;
    }

    private void CleanUp()
    {
        _cellLocker.Locked -= OnCellLocked;
        _cellLocker.Unlocked -= OnCellUnlocked;
    }
}