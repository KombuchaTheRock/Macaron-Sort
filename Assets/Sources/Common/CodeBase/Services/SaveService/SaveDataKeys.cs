using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress;

namespace Sources.Common.CodeBase.Services.SaveService
{
    public static class SaveDataKeys
    {
        private static readonly Dictionary<Type, string> DataKeys = new()
        {
            { typeof(PlayerData), "PlayerData" },
            { typeof(WorldData), "WorldData" },
        };
    
        public static string GetKey<TData>() where TData : ISaveData
        {
            Type dataType = typeof(TData);
    
            if (DataKeys.TryGetValue(dataType, out string key))
                return key;

            throw new KeyNotFoundException($"Key for save data of type {dataType} not found");
        }
    }
}