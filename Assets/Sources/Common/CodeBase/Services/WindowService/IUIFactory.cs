namespace Sources.Common.CodeBase.Services.WindowService
{
    public interface IUIFactory
    {
        void CreateUIRoot();
        void CreateGameOverWindow();
        void CreatePauseWindow();
    }
}