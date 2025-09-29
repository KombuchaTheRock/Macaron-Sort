using Cysharp.Threading.Tasks;

namespace Sources.Common.CodeBase.Services.Settings
{
    public interface IGameSettings
    {
        void MuteSounds();
        void UnMuteSounds();
        void EnableNumbersOnTiles();
        void DisableNumbersOnTiles();
        UniTask LoadSettings();
        void ApplySettings();
        UniTask SaveSettings(GameSettingsData gameSettingsData);
        GameSettingsData GameSettingsData { get; }
    }
}