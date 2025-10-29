using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class PersistentProgressData : BaseProgressData
    {
        public PersistentProgressData(PlayerData playerData, WorldData worldData) : 
            base(playerData, worldData) { }
    }
}