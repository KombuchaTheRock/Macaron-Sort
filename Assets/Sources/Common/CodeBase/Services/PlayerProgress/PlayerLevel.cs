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
            int remainingScore = score;
            
            while (remainingScore > 0)
            {
                int neededForNextLevel = MaxScore - Score;
        
                if (remainingScore >= neededForNextLevel)
                {
                    Level++;
                    remainingScore -= neededForNextLevel;
                    Score = 0;
                    MaxScore = CalculateMaxScore(Level);
                    
                    if (IsLevelControlPoint) 
                        SaveControlPointData();
                }
                else
                {
                    Score += remainingScore;
                    remainingScore = 0;
                }
            }
            
            _gameProgressService.GameProgress.PersistentProgressData.PlayerData.UpdateData(this);
            
            ScoreChanged?.Invoke(Score);
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
                SaveControlPointData();
            
            Debug.Log("OnProgressLoaded");
            ScoreChanged?.Invoke(Score);
        }
    }
}