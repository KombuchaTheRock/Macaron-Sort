using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackSelectionLogic
    {
        bool TrySelectStack(Ray ray, out HexagonStack stack);
        void ResetSelection();
    }
}