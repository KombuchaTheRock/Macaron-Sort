using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TileScoreCellLockView : CellLockView
{
    [SerializeField] private TextMeshProUGUI _scoreToUnlock;
    [SerializeField] private Image _hexagonSprite;

    private TileScoreCellLock _tileScoreCellLock;
    private Dictionary<HexagonTileType, Color> _hexagonColors = new();
    private IStaticDataService _staticData;

    [Inject]
    private void Construct(IStaticDataService staticData) =>
        _staticData = staticData;

    public void SetCellLock(TileScoreCellLock tileScoreCellLock)
    {
        if (_hexagonColors.Count == 0) 
            InitializeHexagonColors();

        if (_tileScoreCellLock != null)
            CleanUp();

        _tileScoreCellLock = tileScoreCellLock;
        _scoreToUnlock.text = _tileScoreCellLock.ScoreToUnlock.ToString();
        _hexagonSprite.color = _hexagonColors[_tileScoreCellLock.TileType];

        SubscribeUpdates();
    }

    private void InitializeHexagonColors()
    {
        HexagonTileType[] hexagonTileTypes = (HexagonTileType[])Enum.GetValues(typeof(HexagonTileType));

        foreach (HexagonTileType hexagonTileType in hexagonTileTypes)
        {
            Color hexagonColor = _staticData.ForHexagonTile(hexagonTileType).HexagonMaterial.color;
            _hexagonColors.Add(hexagonTileType, hexagonColor);
        }
    }

    private void OnScoreToUnlockChanged(int scoreToUnlock) =>
        _scoreToUnlock.text = scoreToUnlock.ToString();

    private void SubscribeUpdates() =>
        _tileScoreCellLock.ScoreToUnlockChanged += OnScoreToUnlockChanged;

    private void CleanUp() =>
        _tileScoreCellLock.ScoreToUnlockChanged -= OnScoreToUnlockChanged;
}