using System.Collections.Generic;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileScoreCellLockView : CellLockView
{
    [SerializeField] private TextMeshProUGUI _scoreToUnlock;
    [SerializeField] private Image _hexagonSprite;
    
    private TileScoreCellLock _tileScoreCellLock;
    private Dictionary<HexagonTileType, Color> _hexagonColors;

    public void Initialize(TileScoreCellLock tileScoreCellLock)
    {
        if (_tileScoreCellLock != null) 
            CleanUp();
        
        _tileScoreCellLock = tileScoreCellLock;
        _scoreToUnlock.text = _tileScoreCellLock.ScoreToUnlock.ToString();
        
        SubscribeUpdates();
    }

    private void OnScoreToUnlockChanged(int scoreToUnlock) => 
        _scoreToUnlock.text = scoreToUnlock.ToString();

    private void SubscribeUpdates() => 
        _tileScoreCellLock.ScoreToUnlockChanged += OnScoreToUnlockChanged;

    private void CleanUp() => 
        _tileScoreCellLock.ScoreToUnlockChanged -= OnScoreToUnlockChanged;
}
