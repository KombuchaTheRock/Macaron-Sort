using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.SaveService;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.Settings
{
    public class SettingsSaveLoader : ISettingsSaveLoader
    {
        private const string SettingsKey = "Settings";
    
        private readonly ISerializer _serializer;

        public SettingsSaveLoader(ISerializer serializer) => 
            _serializer = serializer;

        public async UniTask SaveSettings(GameSettingsData settingsData)
        {
            string serializedData = await _serializer.SerializeAsync(settingsData);
            PlayerPrefs.SetString(SettingsKey, serializedData);
        }

        public async UniTask<GameSettingsData> LoadSettings()
        {
            string savedSettingsJson = PlayerPrefs.GetString(SettingsKey);

            if (savedSettingsJson == null)
            {
                Debug.Log("No settings saved!");
                return null;
            }
            
            return await _serializer.DeserializeAsync<GameSettingsData>(savedSettingsJson);
        }
    }
}