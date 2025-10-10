using System;
using Sources.Features.HexagonSort.BoosterSystem.Activation;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class BoosterCount
    {
        public BoosterType Type;
        public int Count;

        public BoosterCount(BoosterType type, int count)
        {
            Type = type;
            Count = count;
        }
    }
}