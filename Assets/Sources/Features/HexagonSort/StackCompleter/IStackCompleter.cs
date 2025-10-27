using System;

namespace Sources.Features.HexagonSort.StackCompleter
{
    public interface IStackCompleter
    {
        public event Action<HexagonStackScore> StackCompleted;
        event Action DeleteAnimationCompleted;
        void Activate();
        void Deactivate();
        void Reset();
    }
}