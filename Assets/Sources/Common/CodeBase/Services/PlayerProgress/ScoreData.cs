using System;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class ScoreData
    {
        public event Action<int> Changed;
        public int Score;
        public int MaxScore;

        public ScoreData(int maxScore) => 
            MaxScore = maxScore;

        public void AddScore(int score)
        {
            Score += score;
            Changed?.Invoke(score);
        }
    }
}