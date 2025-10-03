using Sources.Common.CodeBase.Services.SoundService;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;
using Zenject;

public class StackMoverSoundPlayer : MonoBehaviour
{
    [SerializeField] private Sound _stackStartDraggingSound;
    [Space(10), SerializeField] private Sound _stackPlacedSound;

    private ISoundService _soundService;
    private IStackMover _stackMover;

    [Inject]
    private void Construct(ISoundService soundService, IStackMover stackMover)
    {
        _stackMover = stackMover;
        _soundService = soundService;
    }

    private void OnEnable() => 
        SubscribeUpdates();

    private void OnDisable() => 
        CleanUp();

    private void SubscribeUpdates()
    {
        _stackMover.StackPlaced += OnStackPlaced;
        _stackMover.DragStarted += OnDragStarted;
    }

    private void CleanUp()
    {
        _stackMover.StackPlaced -= OnStackPlaced;
        _stackMover.DragStarted -= OnDragStarted;
    }

    private void OnDragStarted() =>
        _soundService.Play(_stackStartDraggingSound);

    private void OnStackPlaced(GridCell obj) =>
        _soundService.Play(_stackPlacedSound);
}