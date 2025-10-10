using System;

namespace Sources.Features.HexagonSort.StackCompleter
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