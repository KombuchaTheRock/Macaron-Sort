using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Common.CodeBase.Services.PlayerProgress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Features.UI.Scripts
{
    public class ScoreProgressBar : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private TextMeshProUGUI _scoreProgress;
        [SerializeField] private TextMeshProUGUI _currentLevel;

        private IPlayerLevel _playerLevel;
        private TweenerCore<float,float,FloatOptions> _scoreAddedAnimation;

        [Inject]
        private void Construct(IPlayerLevel playerLevel) =>
            _playerLevel = playerLevel;


        private void Start()
        {
            _playerLevel.ScoreChanged += OnLevelScoreChanged;
            UpdateViewScore();
        }

        private void OnDestroy() =>
            _playerLevel.ScoreChanged -= OnLevelScoreChanged;

        private void OnLevelScoreChanged(int addedScore) =>
            UpdateViewScore();

        private void UpdateViewScore()
        {
            Debug.Log("Score" + _playerLevel.Score);
            Debug.Log("MaxScore" + _playerLevel.MaxScore);
            
            float newValue = (float)_playerLevel.Score /
                                 _playerLevel.MaxScore;
            
            float durationMultiplier = newValue - _progressBar.value;
            float duration = 3 * durationMultiplier;

            _scoreAddedAnimation?.Complete();
            _scoreAddedAnimation = _progressBar.DOValue(newValue, duration)
                .SetEase(Ease.OutCirc)
                .Play()
                .SetLink(gameObject);

            string scoreProgress = $"{_playerLevel.Score} / {_playerLevel.MaxScore}";
            _scoreProgress.text = scoreProgress;

            if (_currentLevel.text != _playerLevel.Level.ToString())
            {
                _currentLevel.transform.DOPunchScale(Vector3.one * 1.2f, 0.5f, 7).Play();
                _currentLevel.text = _playerLevel.Level.ToString();
            }
        }
    }
}