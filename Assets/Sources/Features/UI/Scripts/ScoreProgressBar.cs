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
        
        private IPlayerLevel _playerLevel;

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
            _progressBar.value =
                (float)_playerLevel.Score / 
                _playerLevel.MaxScore;
            
            string scoreProgress = $"{_playerLevel.Score} / {_playerLevel.MaxScore}";
            _scoreProgress.text = scoreProgress;
        }
    }
}