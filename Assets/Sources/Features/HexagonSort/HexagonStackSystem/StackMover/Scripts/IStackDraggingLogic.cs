using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackDraggingLogic
    {
        void Drag(HexagonStack stack, Ray ray);
        GridCell GetTargetCell();
        void ResetCell();
    }
}