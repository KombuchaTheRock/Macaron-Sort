namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public readonly struct LevelProgressResult
    {
        public int Level { get; }
        public int Score { get; }
        public int MaxScore { get; }
        public bool ReachedControlPoint { get; }

        public LevelProgressResult(int level, int score, int maxScore, bool reachedControlPoint)
        {
            Level = level;
            Score = score;
            MaxScore = maxScore;
            ReachedControlPoint = reachedControlPoint;
        }
    }
}