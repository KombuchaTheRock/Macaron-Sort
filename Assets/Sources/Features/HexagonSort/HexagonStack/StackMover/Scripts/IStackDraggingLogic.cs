using Sources.Features.HexagonSort.Grid.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts
{
    public interface IStackDraggingLogic
    {
        void Drag(StackGenerator.Scripts.HexagonStack stack, Ray ray);
        GridCell GetTargetCell();
        void ResetCell();
    }
}