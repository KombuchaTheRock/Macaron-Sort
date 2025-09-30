using Sources.Common.CodeBase.Services.WindowService;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseButton : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    
    private IWindowService _windowService;
    private IPauseService _pauseService;

    [Inject]
    private void Construct(IWindowService windowService, IPauseService pauseService)
    {
        _pauseService = pauseService;
        _windowService = windowService;
    }

    private void Awake()
    {
        _pauseButton.onClick.AddListener(Pause);
    }

    private void OnDestroy() => 
        _pauseButton.onClick.RemoveAllListeners();

    private void Pause()
    {
        _pauseService.Pause();
        _windowService.Open(WindowID.Pause);
    }
}
