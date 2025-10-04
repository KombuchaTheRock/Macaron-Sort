using System;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts
{
    public class GridRotatorMediator : MonoBehaviour
    {
        [SerializeField] private GridRotator _gridRotator;
        
        private IStackMover _stackMover;
        private IStackMerger _stackMerger;

        [Inject]
        private void Construct(IStackMover stackMover, IStackMerger stackMerger)
        {
            _stackMover = stackMover;
            _stackMerger = stackMerger;
        }

        private void Awake() => 
            SubscribeUpdates();

        private void OnDestroy() => 
            CleanUp();

        private void SubscribeUpdates()
        {
            _stackMover.DragStarted += OnDragStarted;
            _stackMover.DragFinished += OnDragFinished;

            _stackMerger.MergeStarted += OnMergeStarted;
            _stackMerger.MergeFinished += OnMergeFinished;
        }

        private void CleanUp()
        {
            _stackMover.DragStarted -= OnDragStarted;
            _stackMover.DragFinished -= OnDragFinished;

            _stackMerger.MergeStarted -= OnMergeStarted;
            _stackMerger.MergeFinished -= OnMergeFinished;
        }

        private void OnMergeStarted() =>
            UpdateGridRotatorEnabled();

        private void OnMergeFinished() =>
            UpdateGridRotatorEnabled();

        private void OnDragFinished() =>
            UpdateGridRotatorEnabled();

        private void OnDragStarted() =>
            UpdateGridRotatorEnabled();

        private void UpdateGridRotatorEnabled()
        {
            if (_stackMover.IsDragging || _stackMerger.IsMerging)
                _gridRotator.enabled = false;
            else
                _gridRotator.enabled = true;
        }
    }
}