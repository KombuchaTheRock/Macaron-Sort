using System;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class Progress
    {
        public ScoreData ScoreData;

        public Progress()
        {
            ScoreData = new ScoreData(100);
        }
    }
}