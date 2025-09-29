using Cysharp.Threading.Tasks;

namespace Sources.Common.CodeBase.Services.Settings
{
    public interface ISettingsSaveLoader
    {
        UniTask SaveSettings(GameSettingsData settingsData);
        UniTask<GameSettingsData> LoadSettings();
    }
}