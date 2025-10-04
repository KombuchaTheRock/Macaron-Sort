using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public class StackMoverSoundPlayer : MonoBehaviour
    {
        private ISoundService _soundService;
        private IStackMover _stackMover;
        private SoundsStaticData _sounds;

        [Inject]
        private void Construct(ISoundService soundService, IStackMover stackMover, IStaticDataService staticData)
        {
            _sounds = staticData.SoundsData;
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
            _soundService.Play(_sounds.StackStartDraggingSound);

        private void OnStackPlaced(GridCell obj) =>
            _soundService.Play(_sounds.StackPlacedSound);
    }
}