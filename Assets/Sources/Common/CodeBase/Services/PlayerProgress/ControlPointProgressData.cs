using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class ControlPointProgressData : ISaveData
    {
        [field: SerializeField] public PlayerData PlayerData { get; private set; }
        [field: SerializeField] public WorldData WorldData { get; private set; }

        public ControlPointProgressData(PlayerData playerData, WorldData worldData)
        {
            PlayerData = playerData;
            WorldData = worldData;
        }
    }
}