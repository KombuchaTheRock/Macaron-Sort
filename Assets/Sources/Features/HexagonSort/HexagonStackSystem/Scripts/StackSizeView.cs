using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Common.CodeBase.Services.Settings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.Scripts
{
    public class StackSizeView : MonoBehaviour, ISettingsReader
    {
        [SerializeField] private TextMeshPro _text;
        
        private HexagonStack _stack;
        private TweenerCore<Vector3, Vector3, VectorOptions> _scaleAnimation;
        private IGameSettings _gameSettings;

        [Inject]
        private void Construct(IGameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        private void Start() => 
            LoadSettings(_gameSettings.GameSettingsData);

        public void Initialize(HexagonStack stack)
        {
            _stack = stack;
            _stack.SizeChanged += OnSizeChanged;
        }

        private void OnDestroy() => 
            _stack.SizeChanged -= OnSizeChanged;

        private void OnSizeChanged() => 
            ChangeText();

        private void ChangeText()
        {
            int sameHexagonsCount = 0;
            
            for (int i = _stack.Hexagons.Count - 1; i >= 0; i--)
            {
                if (_stack.Hexagons[i].TileType != _stack.TopHexagon.TileType)
                    break;
                
                sameHexagonsCount++;
            }

            float newY = _stack.Hexagons.Count * _stack.OffsetBetweenTiles + 0.5f;
            Vector3 newPosition = new(_text.transform.position.x, newY, _text.transform.position.z);

            _text.text = sameHexagonsCount.ToString();
            _text.transform.position = newPosition;
        }

        private void Update() =>
            RotateText();

        private void RotateText()
        {
            if (_text.transform.rotation.y != 0)
                _text.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        public void Hide()
        {
            if (_stack != null && _text.enabled) 
                TextScaleAnim(from: 1, to: 0);
        }

        public void Show()
        {
            if (_stack != null && _text.enabled) 
                TextScaleAnim(from: 0, to: 1);
        }

        private void TextScaleAnim(float from, float to, Action onCompleted = null)
        {
            _scaleAnimation?.Complete();

            _scaleAnimation = _text.transform.DOScale(to, 0.2f)
                .From(from)
                .SetEase(Ease.InOutSine)
                .SetLink(_text.gameObject)
                .OnComplete(() => onCompleted?.Invoke())
                .Play();
        }

        public void LoadSettings(GameSettingsData settings)
        {
            bool numbersEnabled = settings.NumbersOnTilesEnabled;

            if (_text == null)
                return;
            
            _text.enabled = numbersEnabled;
        }
    }
}