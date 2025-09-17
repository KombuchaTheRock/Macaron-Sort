using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public interface IStackDraggingLogic
    {
        void Drag(HexagonStack stack, Ray ray);
        GridCell GetTargetCell();
        void ResetCell();
    }
}