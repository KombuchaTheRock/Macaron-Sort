using Sources.Common.CodeBase.Services.Settings;
using Zenject;

namespace Sources.Features.UI.Scripts
{
    public class NumbersOnTilesToggle : ToggleButton
    {
        private IGameSettings _gameSettings;

        [Inject]
        private void Construct(IGameSettings gameSettings) =>
            _gameSettings = gameSettings;

        private void Start() =>
            LoadSettings(_gameSettings.GameSettingsData);

        protected override void ToggleOn() =>
            _gameSettings.EnableNumbersOnTiles();

        protected override void ToggleOff() =>
            _gameSettings.DisableNumbersOnTiles();

        private void LoadSettings(GameSettingsData gameSettings)
        {
            if (gameSettings == null)
                return;

            IsOn = gameSettings.NumbersOnTilesEnabled;
            UpdateVisualState();
        }
    }
}