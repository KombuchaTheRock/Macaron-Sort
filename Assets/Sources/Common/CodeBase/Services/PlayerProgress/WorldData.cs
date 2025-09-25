using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class WorldData
    {
        [field: SerializeField] public StacksData StacksData { get; private set; }

        public WorldData()
        {
            StacksData = new StacksData();
        }

        public WorldData(StacksData stacksData)
        {
            StacksData = stacksData;
        }
    }
}