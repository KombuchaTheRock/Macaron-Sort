using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackSelectionLogic
    {
        bool TrySelectFreeStack(Ray ray, out HexagonStack stack);
        void ResetSelection();
        bool TrySelectStackOnGrid(Ray ray, out HexagonStack stack, out GridCell gridCell);
    }
}