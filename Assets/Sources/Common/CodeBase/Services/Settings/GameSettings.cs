using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.SoundService;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Settings
{
    public class GameSettings : IGameSettings
    {
        private readonly ISoundService _soundService;
        private readonly IHexagonFactory _hexagonFactory;
        private readonly ISettingsSaveLoader _settingsSaveLoader;

        private bool _soundEnabled;
        private bool _numbersOnTilesEnabled;

        public GameSettingsData GameSettingsData { get; private set; }

        public GameSettings(ISoundService soundService, IHexagonFactory hexagonFactory,
            ISettingsSaveLoader settingsSaveLoader)
        {
            _soundService = soundService;
            _hexagonFactory = hexagonFactory;
            _settingsSaveLoader = settingsSaveLoader;
        }

        public async UniTask LoadSettings()
        {
            GameSettingsData = await _settingsSaveLoader.LoadSettings();

            if (GameSettingsData != null)
            {
                _soundEnabled = GameSettingsData.SoundEnabled;
                _numbersOnTilesEnabled = GameSettingsData.NumbersOnTilesEnabled;    
            }
            else
            {
                await InitializeNewSettings();
            }
        }

        public async UniTask SaveSettings(GameSettingsData gameSettingsData) =>
            await _settingsSaveLoader.SaveSettings(gameSettingsData);

        private async UniTask InitializeNewSettings()
        {
            Debug.Log("Initializing Game Settings");
            
            GameSettingsData = new GameSettingsData(true, true);
            
            _soundEnabled = GameSettingsData.SoundEnabled;
            _numbersOnTilesEnabled = GameSettingsData.NumbersOnTilesEnabled;    
            
            await SaveSettings(GameSettingsData);
        }

        public void MuteSounds()
        {
            _soundEnabled = false;
            ApplySettings();
        }

        public void UnMuteSounds()
        {
            _soundEnabled = true;
            ApplySettings();
        }

        public void EnableNumbersOnTiles()
        {
            _numbersOnTilesEnabled = true;
            ApplySettings();
        }

        public void DisableNumbersOnTiles()
        {
            _numbersOnTilesEnabled = false;
            ApplySettings();
        }

        public void ApplySettings()
        {
           GameSettingsData = new(_soundEnabled, _numbersOnTilesEnabled);

            foreach (ISettingsReader settingsReader in _hexagonFactory.SettingsReaders) 
                settingsReader?.LoadSettings(GameSettingsData);

            if (_soundEnabled)
                _soundService.UnMute();
            else
                _soundService.Mute();
            
            SaveSettings(GameSettingsData).Forget();
        }
    }
}