using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackMover
    {
        event Action DragStarted;
        event Action DragFinished;
        event Action<GridCell> StackPlaced;
        bool IsDragging { get; }
        GridCell InitialCell { get; }
        void ActivateOnGridSelection();
        void DeactivateOnGridSelection();
        void Activate();
        void Deactivate();
        event Action StackMoved;
    }
}