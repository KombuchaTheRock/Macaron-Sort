using System;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory) => 
            _uiFactory = uiFactory;

        public void Open(WindowID windowID) => 
            CreateWindow(windowID);

        private void CreateWindow(WindowID windowID)
        {
            switch (windowID)
            {
                case WindowID.GameOver:
                    _uiFactory.CreateGameOverWindow();
                    break;
                case WindowID.Pause:
                    _uiFactory.CreatePauseWindow();
                    break;
                case WindowID.RocketBooster:
                    _uiFactory.CreateRocketBoosterWindow();
                    break;
                case WindowID.ArrowBooster:
                    _uiFactory.CreateArrowBoosterWindow();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(windowID), windowID, null);
            }
        }
    }
}