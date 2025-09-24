using System;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class PlayerLevel : IDisposable, IPlayerLevel
    {
        public event Action<int> ScoreChanged;

        private readonly IGameProgressService _gameProgressService;
        public int Level { get; private set; }
        public int Score { get; private set; }

        public int MaxScore { get; private set; } = 100;

        public PlayerLevel(IGameProgressService gameProgressService)
        {
            _gameProgressService = gameProgressService;
            _gameProgressService.ProgressLoaded += OnProgressLoaded;
        }
        
        public void AddScore(int score)
        {
            Score += score;
            
            _gameProgressService.GameProgress.PlayerData.UpdateData(this);
            ScoreChanged?.Invoke(score);
        }
    
        public void Dispose() => 
            _gameProgressService.ProgressLoaded -= OnProgressLoaded;
    
        private void OnProgressLoaded()
        {
            Score = _gameProgressService.GameProgress.PlayerData.Score;
            Level = _gameProgressService.GameProgress.PlayerData.Level;
            
            ScoreChanged?.Invoke(Score);
        }
    }
}