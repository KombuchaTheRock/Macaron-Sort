using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class WindowAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration;
        
        public void ImageFadeAnimation(Image image, float from, float to,
            Action onCompleted = null)
        {
            image.DOFade(to, _duration)
                .From(from)
                .SetLink(gameObject)
                .SetUpdate(true)
                .OnComplete(() => onCompleted?.Invoke())
                .Play();
        }

        public void TextFadeAnimation(TextMeshProUGUI text, float from, float to,
            Action onCompleted = null)
        {
            text.DOFade(to, _duration)
                .From(from)
                .SetLink(gameObject)
                .SetUpdate(true)
                .OnComplete(() => onCompleted?.Invoke())
                .Play();
        }
    }
}