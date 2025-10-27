using System;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.StackCompleter;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public interface IStackMerger
    {
        event Action MergeAnimationCompleted;
        event Action HexagonDeleteAnimationCompleted;
        public event Action<HexagonStackScore> StackCompleted;
        event Action MergeStarted;
        event Action MergeFinished;
        bool IsMerging { get; }
        void Initialize(HexagonGrid hexagonGrid);
        void UpdateOccupiedCells();
    }
}