using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.BoosterSystem.Activation;

namespace Sources.Features.HexagonSort.BoosterSystem.Counter
{
    public interface IBoosterCounter
    {
        void AddBoosterAmount(BoosterType boosterType, int amount);
        void RemoveBooster(BoosterType boosterType);
        event Action<Dictionary<BoosterType, int>> BoosterCountChanged;
        Dictionary<BoosterType, int> BoostersCount { get; }
    }
}