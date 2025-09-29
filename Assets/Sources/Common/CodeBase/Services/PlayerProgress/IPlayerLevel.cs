using System;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public interface IPlayerLevel
    {
        event Action<int> ScoreChanged;
        event Action ControlPointAchieved;
        int Level { get; }
        int Score { get; }
        int MaxScore { get; }
        void AddScore(int score);
    }
}