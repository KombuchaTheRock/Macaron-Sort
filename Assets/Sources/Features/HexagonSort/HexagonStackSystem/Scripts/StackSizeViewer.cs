using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.Scripts
{
    public class StackSizeViewer : MonoBehaviour
    {
        private HexagonStack _stack;
        [SerializeField] private TextMeshPro _text;
        private TweenerCore<Vector3, Vector3, VectorOptions> _scaleAnimation;

        public void Initialize(HexagonStack stack)
        {
            _stack = stack;
            _stack.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged()
        {
            Hide();
            ChangeText();
        }

        private void ChangeText()
        {
            int hexagonsCount = _stack.Hexagons.Count;

            float newY = hexagonsCount * _stack.OffsetBetweenTiles + 0.5f;
            Vector3 newPosition = new(_text.transform.position.x, newY, _text.transform.position.z);

            _text.text = hexagonsCount.ToString();
            _text.transform.position = newPosition;
        }

        private void Update() =>
            RotateText();

        private void RotateText()
        {
            if (_text.transform.rotation.y != 0)
                _text.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        public void Hide(Action onCompleted = null)
        {
            if (_stack == null)
                TextScaleAnim(_stack.Hexagons.Count, from: 1, to: 0,
                    () =>
                    {
                        onCompleted?.Invoke();
                        //_text.gameObject.SetActive(false);
                    });
        }

        public void Show()
        {
            //_text.gameObject.SetActive(true);

            if (_stack == null)
                TextScaleAnim(_stack.Hexagons.Count, from: 0, to: 1);
        }


        private void TextScaleAnim(int hexagonsCount, float from, float to, Action onCompleted = null)
        {
            _scaleAnimation?.Complete();

            _scaleAnimation = _text.transform.DOScale(to, 0.5f)
                .From(from)
                .SetEase(Ease.OutBounce)
                .SetLink(gameObject)
                .OnComplete(() => onCompleted?.Invoke())
                .Play();
        }
    }
}