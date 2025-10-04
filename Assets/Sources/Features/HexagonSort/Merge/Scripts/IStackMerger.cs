using System;
using Sources.Features.HexagonSort.GridSystem.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public interface IStackMerger
    {
        event Action MergeAnimationCompleted;
        event Action HexagonDeleteAnimationCompleted;
        event Action<int> StackCompleted;
        event Action MergeStarted;
        event Action MergeFinished;
        bool IsMerging { get; }
        void Initialize(HexagonGrid hexagonGrid);
        void UpdateOccupiedCells();
    }
}