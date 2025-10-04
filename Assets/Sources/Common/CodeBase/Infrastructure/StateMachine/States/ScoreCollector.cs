using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class ScoreCollector : MonoBehaviour
    {
        private IPlayerLevel _playerLevel;
        private IStackMerger _stackMerger;

        [Inject]
        private void Construct(IPlayerLevel playerLevel, IStackMerger stackMerger)
        {
            _playerLevel = playerLevel;
            _stackMerger = stackMerger;
        }

        private void Awake() => 
            SubscribeUpdates();

        private void OnDestroy() =>
            CleanUp();

        private void SubscribeUpdates() =>
            _stackMerger.StackCompleted += OnStackCompleted;

        private void CleanUp() =>
            _stackMerger.StackCompleted -= OnStackCompleted;

        private void OnStackCompleted(int score) => 
            _playerLevel.AddScore(score);
    }
}