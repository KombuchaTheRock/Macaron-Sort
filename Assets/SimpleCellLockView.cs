using TMPro;
using UnityEngine;

public class SimpleCellLockView : CellLockView
{
    [SerializeField] private TextMeshPro _textMeshPro;
    
    private SimpleCellLock _simpleCellLock;

    public void SetCellLock(SimpleCellLock simpleCellLock)
    {
        if (_simpleCellLock != null) 
            CleanUp();
        
        _simpleCellLock = simpleCellLock;
        _textMeshPro.text = _simpleCellLock.CompletedStacksToUnlock.ToString();
        
        SubscribeUpdates();
    }
    
    private void OnStackToUnlockCountChanged(int count)
    {
        _textMeshPro.text = count.ToString();
    }

    private void SubscribeUpdates()
    {
        _simpleCellLock.StackToUnlockCountChanged += OnStackToUnlockCountChanged;
    }

    private void CleanUp()
    {
        _simpleCellLock.StackToUnlockCountChanged -= OnStackToUnlockCountChanged;
    }
}