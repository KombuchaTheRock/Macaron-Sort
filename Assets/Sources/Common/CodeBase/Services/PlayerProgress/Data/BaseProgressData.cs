using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public abstract class BaseProgressData : ISaveData
    {
        [field: SerializeField] public PlayerData PlayerData { get; protected set; }
        [field: SerializeField] public WorldData WorldData { get; protected set; }

        protected BaseProgressData(PlayerData playerData, WorldData worldData)
        {
            PlayerData = playerData;
            WorldData = worldData;
        }

        public T CloneTo<T>() where T : BaseProgressData
        {
            string json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<T>(json);
        }
    }
}