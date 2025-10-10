namespace Sources.Common.CodeBase.Services.WindowService
{
    public interface IUIFactory
    {
        void CreateUIRoot();
        WindowBase CreateGameOverWindow();
        WindowBase CreatePauseWindow();
        WindowBase CreateRocketBoosterWindow();
        WindowBase CreateArrowBoosterWindow();
    }
}