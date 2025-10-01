using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class ScoreCollector : MonoBehaviour
    {
        private IPlayerLevel _playerLevel;
        private MergeSystem _mergeSystem;

        [Inject]
        private void Construct(IPlayerLevel playerLevel) => 
            _playerLevel = playerLevel;

        public void Initialize(MergeSystem mergeSystem)
        {
            _mergeSystem = mergeSystem;
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            CleanUp();

        private void SubscribeUpdates() =>
            _mergeSystem.StackCompleted += OnStackCompleted;

        private void CleanUp() =>
            _mergeSystem.StackCompleted -= OnStackCompleted;

        private void OnStackCompleted(int score) => 
            _playerLevel.AddScore(score);
    }
}