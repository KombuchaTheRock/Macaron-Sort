using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Features.HexagonSort.BoosterSystem.Activation;
using UnityEngine;

namespace Sources.Features.HexagonSort.BoosterSystem.Counter
{
    public class BoosterCounter : IDisposable, IBoosterCounter
    {
        public event Action<Dictionary<BoosterType, int>> BoosterCountChanged;

        public Dictionary<BoosterType, int> BoostersCount { get; private set; } = new();

        private readonly IGameProgressService _gameProgressService;
        private readonly IPlayerLevel _playerLevel;

        public BoosterCounter(IGameProgressService gameProgressService, IPlayerLevel playerLevel)
        {
            _gameProgressService = gameProgressService;
            _playerLevel = playerLevel;
        
            SubscribeUpdates();
        }

        public void Dispose() => 
            CleanUp();

        public void AddBoosterAmount(BoosterType boosterType, int amount)
        {
            BoostersCount[boosterType] += amount;
        
            UpdateBoosterPersistentData();
            BoosterCountChanged?.Invoke(BoostersCount);
        }

        public void RemoveBooster(BoosterType boosterType)
        {
            if (BoostersCount[boosterType] > 0)
                BoostersCount[boosterType]--;

            UpdateBoosterPersistentData();
            BoosterCountChanged?.Invoke(BoostersCount);
        }

        private void UpdateBoosterPersistentData()
        {
            PlayerData playerData = _gameProgressService.GameProgress.PersistentProgressData.PlayerData;
        
            List<BoosterCount> boosterData = BoostersCount.Select(pair => new BoosterCount(pair.Key, pair.Value)).ToList();
            playerData.UpdateBoosterData(boosterData);
        }

        private void UpdateBoosterControlPointData()
        {
            PlayerData playerData = _gameProgressService.GameProgress.ControlPointProgressData.PlayerData;
        
            List<BoosterCount> boosterData = BoostersCount.Select(pair => new BoosterCount(pair.Key, pair.Value)).ToList();
            playerData.UpdateBoosterData(boosterData);
        }

        private void OnProgressLoaded()
        {
            Debug.Log("OnProgressLoaded");
            Debug.Log($"BoosterCount: {BoostersCount.Count}");
            IReadOnlyList<BoosterCount> playerDataBoosters = _gameProgressService
                .GameProgress.PersistentProgressData.PlayerData.Boosters;
        
            BoostersCount = playerDataBoosters.ToDictionary(x => x.Type, x => x.Count);
            BoosterCountChanged?.Invoke(BoostersCount);
        }

        private void SubscribeUpdates()
        {
            _gameProgressService.ProgressLoaded += OnProgressLoaded;
            _playerLevel.ControlPointAchieved += OnControlPointAchieved;
        }

        private void OnControlPointAchieved()
        {
            if (_playerLevel.Level == 1)
                return;
            
            AddBoosterAmount(BoosterType.ArrowBooster, 5);
            AddBoosterAmount(BoosterType.RocketBooster, 5);
            AddBoosterAmount(BoosterType.ReverseBooster, 5);
            
            UpdateBoosterControlPointData();
        }

        private void CleanUp()
        {
            _gameProgressService.ProgressLoaded -= OnProgressLoaded;
            _playerLevel.ControlPointAchieved -= OnControlPointAchieved;
        }
    }
}