using TMPro;
using UnityEngine;
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

        [Inject]
        private void Construct(IPauseService pauseService) =>
            _pauseService = pauseService;

        protected override void OnAwake()
        {
            foreach (Image image in _animatedImages)
                _windowAnimation.ImageFadeAnimation(image, 0, image.color.a);

            foreach (TextMeshProUGUI text in _animatedTexts)
                _windowAnimation.TextFadeAnimation(text, 0, text.color.a);

            base.OnAwake();
        }

        protected override void OnClicked()
        {
            foreach (TextMeshProUGUI text in _animatedTexts)
                _windowAnimation.TextFadeAnimation(text, text.color.a, 0);

            foreach (Image image in _animatedImages)
                _windowAnimation.ImageFadeAnimation(image, image.color.a, 0, () =>
                    {
                        _pauseService.Unpause();
                        base.OnClicked();
                    }
                );
        }
    }
}