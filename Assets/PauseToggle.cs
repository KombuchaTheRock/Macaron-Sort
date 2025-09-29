using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.UI.Scripts;
using Zenject;

public class PauseToggle : ToggleButton
{
    private WindowService _windowService;

    [Inject]
    private void Construct(WindowService windowService)
    {
        _windowService = windowService;
    }  
}
