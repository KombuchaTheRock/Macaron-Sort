using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Zenject;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private WindowAnimation _windowAnimation;
        [SerializeField] private Image[] _animatedImages;
        [SerializeField] private TextMeshProUGUI[] _animatedTexts;

        private IPauseService _pauseService;
        private PostProcessVolume _postProcessVolume;

        [Inject]
        private void Construct(IPauseService pauseService) =>
            _pauseService = pauseService;

        protected override void OnAwake()
        {
            _postProcessVolume = Camera.main.gameObject.GetComponent<PostProcessVolume>();
            _postProcessVolume.enabled = true;

            StartCoroutine(BlurRoutine(0, 1, 0.01f));

            foreach (Image image in _animatedImages)
                _windowAnimation.ImageFadeAnimation(image, 0, image.color.a);

            foreach (TextMeshProUGUI text in _animatedTexts)
                _windowAnimation.TextFadeAnimation(text, 0, text.color.a);

            base.OnAwake();
        }

        private IEnumerator BlurRoutine(float from, float to, float timeBetweenSteps, Action onCompleted = null)
        {
            _postProcessVolume.weight = from;
            
            int sign = from > to ? -1 : 1;
            
            while (from <= to)
            {
                from += 0.001f * sign;
                _postProcessVolume.weight = from;
                yield return new WaitForSecondsRealtime(timeBetweenSteps);
            }
            
            onCompleted?.Invoke();
        }

        protected override void OnCloseButtonClicked()
        {
            foreach (TextMeshProUGUI text in _animatedTexts)
                _windowAnimation.TextFadeAnimation(text, text.color.a, 0);

            foreach (Image image in _animatedImages)
                _windowAnimation.ImageFadeAnimation(image, image.color.a, 0, () =>
                    {
                        StartCoroutine(BlurRoutine(1, 0, 0.01f, () =>
                        {
                            _postProcessVolume.enabled = false;
                            _pauseService.Unpause();
                            base.OnCloseButtonClicked();    
                        }));
                    }
                );
        }
    }
}