using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.Merge.Scripts;
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
        
        private MergeSystem _mergeSystem;
        private IPlayerProgress _playerProgress;

        [Inject]
        private void Construct(IPlayerProgress playerProgress) =>
            _playerProgress = playerProgress;

        private void Start()
        {
            _playerProgress.Progress.ScoreData.Changed += OnScoreChanged;
            UpdateScore();
        }

        private void OnDestroy() =>
            _playerProgress.Progress.ScoreData.Changed -= OnScoreChanged;

        private void OnScoreChanged(int addedScore) => 
            UpdateScore();

        private void UpdateScore()
        {
            _progressBar.value =
                (float)_playerProgress.Progress.ScoreData.Score / 
                _playerProgress.Progress.ScoreData.MaxScore;
            
            string scoreProgress = $"{_playerProgress.Progress.ScoreData.Score} / {_playerProgress.Progress.ScoreData.MaxScore}";
            _scoreProgress.text = scoreProgress;
        }
    }
}