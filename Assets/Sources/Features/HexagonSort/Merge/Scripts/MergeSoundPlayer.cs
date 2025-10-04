using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSoundPlayer : MonoBehaviour
    {
        private ISoundService _soundService;
        private SoundsStaticData _sounds;
        private IStackMerger _stackMerger;

        [Inject]
        private void Construct(ISoundService soundService, IStaticDataService staticData, IStackMerger stackMerger)
        {
            _sounds = staticData.SoundsData;
            _soundService = soundService;
            _stackMerger = stackMerger;
        }

        private void Awake() => 
            SubscribeUpdates();

        private void OnDestroy() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _stackMerger.MergeAnimationCompleted += OnMergeAnimationCompleted;
            _stackMerger.HexagonDeleteAnimationCompleted += OnHexagonDeleteAnimationCompleted;
        }

        private void CleanUp()
        {
            _stackMerger.MergeAnimationCompleted -= OnMergeAnimationCompleted;
            _stackMerger.HexagonDeleteAnimationCompleted -= OnHexagonDeleteAnimationCompleted;
        }

        private void OnHexagonDeleteAnimationCompleted() => 
            _soundService.Play(_sounds.HexagonDeleteSound);

        private void OnMergeAnimationCompleted() => 
            _soundService.Play(_sounds.MergeSound);
    }
}