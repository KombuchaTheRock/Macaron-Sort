using System;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory) => 
            _uiFactory = uiFactory;

        public WindowBase Open(WindowID windowID) => 
            CreateWindow(windowID);

        private WindowBase CreateWindow(WindowID windowID)
        {
            switch (windowID)
            {
                case WindowID.GameOver:
                    return _uiFactory.CreateGameOverWindow();
                case WindowID.Pause:
                    return _uiFactory.CreatePauseWindow();
                case WindowID.RocketBooster:
                    return _uiFactory.CreateRocketBoosterWindow();
                case WindowID.ArrowBooster:
                    return _uiFactory.CreateArrowBoosterWindow();
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowID), windowID, null);
            }
        }
    }
}