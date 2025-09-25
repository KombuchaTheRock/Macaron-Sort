using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class PlayerLevel : IDisposable, IPlayerLevel
    {
        public event Action<int> ScoreChanged;
        public event Action ControlPointAchieved;

        private readonly IGameProgressService _gameProgressService;
        private readonly PlayerLevelConfig _config;
        
        public int Level { get; private set; }
        public int Score { get; private set; }
        public int MaxScore { get; private set; }

        private bool IsLevelControlPoint => Level % _config.ControlPointSaveInterval == 0;
        
        public PlayerLevel(IGameProgressService gameProgressService, IStaticDataService staticData)
        {
            _config = staticData.GameConfig.PlayerLevelConfig;
            _gameProgressService = gameProgressService;
            
            _gameProgressService.ProgressLoaded += OnProgressLoaded;
        }
        
        public void AddScore(int score)
        {
            if (score > MaxScore)
                score = MaxScore;
            
            Score += score;

            if (Score >= MaxScore) 
                NextLevel(score);
            
            _gameProgressService.GameProgress.PersistentProgressData.PlayerData.UpdateData(this);
            ScoreChanged?.Invoke(score);
        }

        private void NextLevel(int score)
        {
            Level++;
            Score = score;
            MaxScore = CalculateMaxScore(Level);

            if (IsLevelControlPoint)
            {
                

                SaveControlPointData();
            }
        }

        private void SaveControlPointData()
        {
            Debug.Log("ControlPointSaved");
            
            _gameProgressService.GameProgress.ControlPointProgressData.PlayerData.UpdateData(this);
            _gameProgressService.SaveControlPointProgressAsync();
                
            ControlPointAchieved?.Invoke();
        }

        private int CalculateMaxScore(int level) => 
            Mathf.RoundToInt(_config.BaseMaxScoreForLevel * Mathf.Pow(_config.MaxScoreGrowthFactor, level - 1));

        public void Dispose() => 
            _gameProgressService.ProgressLoaded -= OnProgressLoaded;
    
        private void OnProgressLoaded()
        {
            Score = _gameProgressService.GameProgress.PersistentProgressData.PlayerData.Score;
            Level = _gameProgressService.GameProgress.PersistentProgressData.PlayerData.Level;

            MaxScore = CalculateMaxScore(Level);

            if (Level == 1)
            {
                SaveControlPointData();
            }
            
            ScoreChanged?.Invoke(Score);
        }
    }
}