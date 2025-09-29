using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class PersistentProgressData : ISaveData
    {
        [field: SerializeField] public PlayerData PlayerData { get; private set; }
        [field: SerializeField] public WorldData WorldData { get; private set; }

        public PersistentProgressData(PlayerData playerData, WorldData worldData)
        {
            PlayerData = playerData;
            WorldData = worldData;
        }
    }
}