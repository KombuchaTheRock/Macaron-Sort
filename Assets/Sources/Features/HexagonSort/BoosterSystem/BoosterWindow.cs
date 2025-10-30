using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Services.WindowService;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Features.HexagonSort.BoosterSystem
{
    public class BoosterWindow : WindowBase
    {
        public event Action CloseButtonClicked;

        [SerializeField] private WindowAnimation _windowAnimation;
        [SerializeField] private List<Image> _imagesForAnimation;
        [SerializeField] private List<TextMeshProUGUI> _textsForAnimation;
        [Space(10)]
        [SerializeField] private Image _boosterImage;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [Space(10)]
        [SerializeField] private Sprite _boosterIcon;
        [SerializeField] private string _boosterName;
        [TextArea, SerializeField] private string _boosterDescription;

        protected override void OnAwake()
        {
            base.OnAwake();
            AppearAnimation();

            _boosterImage.sprite = _boosterIcon;
            _name.text = _boosterName;
            _description.text = _boosterDescription;
        }

        private void AppearAnimation()
        {
            foreach (Image image in _imagesForAnimation) 
                _windowAnimation.ImageFadeAnimation(image, 0, 1);

            foreach (TextMeshProUGUI text in _textsForAnimation) 
                _windowAnimation.TextFadeAnimation(text, 0, 1);
        }

        public void CloseWindow() =>
            DisappearAnimation();

        protected override void OnCloseButtonClicked() => 
            DisappearAnimation();

        private void DisappearAnimation()
        {
            foreach (Image image in _imagesForAnimation) 
                _windowAnimation.ImageFadeAnimation(image, image.color.a, 0);

            foreach (TextMeshProUGUI text in _textsForAnimation) 
                _windowAnimation.TextFadeAnimation(text, text.color.a, 0, OnDisappearAnimationCompleted);
        }

        private void OnDisappearAnimationCompleted()
        {
            CloseButtonClicked?.Invoke();
            Destroy(gameObject);
        }
    }
}
