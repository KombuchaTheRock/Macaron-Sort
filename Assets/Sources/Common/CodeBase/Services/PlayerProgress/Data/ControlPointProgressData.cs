using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class ControlPointProgressData : BaseProgressData
    {
        public ControlPointProgressData(PlayerData playerData, WorldData worldData) : 
            base(playerData, worldData) { }
    }
}