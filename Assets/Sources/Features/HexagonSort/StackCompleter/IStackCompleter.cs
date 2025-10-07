using System;

namespace Sources.Features.HexagonSort.StackSelector
{
    public interface IStackCompleter
    {
        event Action<int> StackCompleted;
        event Action DeleteAnimationCompleted;
        void Activate();
        void Deactivate();
        void Reset();
    }
}