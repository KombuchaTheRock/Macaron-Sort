using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class WorldData
    {
        [field: SerializeField] public StacksData StacksData { get; private set; }

        public WorldData(StacksData stacksData) => 
            StacksData = stacksData;
    }
}