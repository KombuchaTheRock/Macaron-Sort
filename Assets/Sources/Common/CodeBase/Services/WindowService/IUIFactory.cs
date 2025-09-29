using System.Collections.Generic;
using Sources.Common.CodeBase.Services.Settings;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public interface IUIFactory
    {
        List<ISettingsReader> SettingsReaders { get; }
        void CreateUIRoot();
        void CreateGameOverWindow();
        void CreatePauseWindow();
    }
}