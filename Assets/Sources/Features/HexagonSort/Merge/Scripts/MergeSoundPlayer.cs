using Sources.Common.CodeBase.Services.SoundService;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSoundPlayer : MonoBehaviour
    {
        [SerializeField] private MergeSystem _mergeSystem;
        [Space(20), SerializeField] private Sound _mergeSound;
        [SerializeField] private Sound _hexagonDeleteSound;

        private ISoundService _soundService;

        [Inject]
        private void Construct(ISoundService soundService) => 
            _soundService = soundService;

        private void Awake() => 
            SubscribeUpdates();

        private void OnDestroy() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _mergeSystem.HexagonMergeAnimationCompleted += OnHexagonMergeAnimationCompleted;
            _mergeSystem.HexagonDeleteAnimationCompleted += OnHexagonDeleteAnimationCompleted;
        }

        private void CleanUp()
        {
            _mergeSystem.HexagonMergeAnimationCompleted -= OnHexagonMergeAnimationCompleted;
            _mergeSystem.HexagonDeleteAnimationCompleted -= OnHexagonDeleteAnimationCompleted;
        }

        private void OnHexagonDeleteAnimationCompleted() => 
            _soundService.Play(_hexagonDeleteSound);

        private void OnHexagonMergeAnimationCompleted() => 
            _soundService.Play(_mergeSound);
    }
}