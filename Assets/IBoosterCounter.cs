using System;
using System.Collections.Generic;

public interface IBoosterCounter
{
    void AddBoosterAmount(BoosterType boosterType, int amount);
    void RemoveBooster(BoosterType boosterType);
    event Action<Dictionary<BoosterType, int>> BoosterCountChanged;
    Dictionary<BoosterType, int> BoostersCount { get; }
}