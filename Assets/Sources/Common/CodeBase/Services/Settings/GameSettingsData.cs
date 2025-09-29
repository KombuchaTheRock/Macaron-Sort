using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Settings
{
    [Serializable]
    public class GameSettingsData
    {
        [field: SerializeField] public bool SoundEnabled { get; private set; }
        [field: SerializeField] public bool NumbersOnTilesEnabled { get; private set; }

        public GameSettingsData(bool soundEnabled, bool numbersOnTilesEnabled)
        {
            SoundEnabled = soundEnabled;
            NumbersOnTilesEnabled = numbersOnTilesEnabled;
        }
    }
}