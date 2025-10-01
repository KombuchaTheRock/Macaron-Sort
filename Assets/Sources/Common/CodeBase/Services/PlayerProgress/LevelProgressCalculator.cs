using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class LevelProgressCalculator
    {
        private readonly PlayerLevelConfig _config;

        public LevelProgressCalculator(PlayerLevelConfig config) => 
            _config = config;

        public LevelProgressResult CalculateLevelProgress(int currentLevel, int currentScore, int maxScore, int addedScore)
        {
            int remainingScore = addedScore;
            int newLevel = currentLevel;
            int newScore = currentScore;
            int newMaxScore = maxScore;
            bool reachedControlPoint = false;

            while (remainingScore > 0)
            {
                int neededForNextLevel = newMaxScore - newScore;

                if (remainingScore >= neededForNextLevel)
                {
                    newLevel++;
                    remainingScore -= neededForNextLevel;
                    newScore = 0;
                    newMaxScore = CalculateMaxScore(newLevel);
                
                    if (IsLevelControlPoint(newLevel))
                        reachedControlPoint = true;
                }
                else
                {
                    newScore += remainingScore;
                    remainingScore = 0;
                }
            }

            return new LevelProgressResult(newLevel, newScore, newMaxScore, reachedControlPoint);
        }

        public int CalculateMaxScore(int level) => 
            Mathf.RoundToInt(_config.BaseMaxScoreForLevel * Mathf.Pow(_config.MaxScoreGrowthFactor, level - 1));

        private bool IsLevelControlPoint(int level) => 
            level % _config.ControlPointSaveInterval == 0;
    }
}