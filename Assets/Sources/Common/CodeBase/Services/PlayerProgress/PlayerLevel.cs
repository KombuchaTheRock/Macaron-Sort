using System;
using Sources.Common.CodeBase.Services.StaticData;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class PlayerLevel : IDisposable, IPlayerLevel
    {
        public event Action<int> ScoreChanged;
        public event Action ControlPointAchieved;
        public event Action<int> LevelChanged;

        private readonly IGameProgressService _gameProgressService;
        private readonly LevelProgressCalculator _levelProgressCalculator;
        private int _level;

        public int Level
        {
            get => _level;
            private set
            {
                if (_level != value)
                    LevelChanged?.Invoke(value);
                    
                _level = value;
            }
        }

        public int Score { get; private set; }
        public int MaxScore { get; private set; }

        public PlayerLevel(IGameProgressService gameProgressService, IStaticDataService staticData)
        {
            PlayerLevelConfig config = staticData.GameConfig.PlayerLevelConfig;
            _gameProgressService = gameProgressService;
            _levelProgressCalculator = new LevelProgressCalculator(config);

            SubscribeUpdates();
        }

        public void Dispose() =>
            CleanUp();

        private void SubscribeUpdates() =>
            _gameProgressService.ProgressLoaded += OnProgressLoaded;

        private void CleanUp() => 
            _gameProgressService.ProgressLoaded -= OnProgressLoaded;

        public void AddScore(int addedScore)
        {
            LevelProgressResult progressResult = _levelProgressCalculator
                .CalculateLevelProgress(
                    Level,
                    Score,
                    MaxScore,
                    addedScore);

            UpdatePlayerState(progressResult);

            if (progressResult.ReachedControlPoint)
                SaveControlPointData();

            _gameProgressService.GameProgress.PersistentProgressData.PlayerData.UpdateLevelData(this);
            ScoreChanged?.Invoke(Score);
        }

        private void UpdatePlayerState(LevelProgressResult result)
        {
            Level = result.Level;
            Score = result.Score;
            MaxScore = result.MaxScore;
        }

        private void SaveControlPointData()
        {
            _gameProgressService.GameProgress.ControlPointProgressData.PlayerData.UpdateLevelData(this);
            _gameProgressService.SaveControlPointProgressAsync();

            ControlPointAchieved?.Invoke();
        }

        private void OnProgressLoaded()
        {
            Score = _gameProgressService.GameProgress.PersistentProgressData.PlayerData.Score;
            Level = _gameProgressService.GameProgress.PersistentProgressData.PlayerData.Level;

            MaxScore = _levelProgressCalculator.CalculateMaxScore(Level);

            if (Level == 1)
                SaveControlPointData();

            ScoreChanged?.Invoke(Score);
        }
    }
}