using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.StackCompleter;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class MergeSoundPlayer : MonoBehaviour
    {
        private ISoundService _soundService;
        private SoundsStaticData _sounds;
        private IStackMerger _stackMerger;
        private IStackCompleter _stackCompleter;

        [Inject]
        private void Construct(ISoundService soundService, IStaticDataService staticData, IStackMerger stackMerger,
            IStackCompleter stackCompleter)
        {
            _stackCompleter = stackCompleter;
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

            _stackCompleter.DeleteAnimationCompleted += OnHexagonDeleteAnimationCompleted;
        }

        private void CleanUp()
        {
            _stackMerger.MergeAnimationCompleted -= OnMergeAnimationCompleted;
            _stackMerger.HexagonDeleteAnimationCompleted -= OnHexagonDeleteAnimationCompleted;
            
            _stackCompleter.DeleteAnimationCompleted -= OnHexagonDeleteAnimationCompleted;
        }

        private void OnHexagonDeleteAnimationCompleted() =>
            _soundService.Play(_sounds.HexagonDeleteSound);

        private void OnMergeAnimationCompleted() =>
            _soundService.Play(_sounds.MergeSound);
    }
}