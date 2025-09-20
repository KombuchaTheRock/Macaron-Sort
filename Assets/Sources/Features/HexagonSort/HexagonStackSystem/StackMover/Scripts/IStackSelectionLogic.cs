using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackSelectionLogic
    {
        bool TrySelectStack(Ray ray, out StackGenerator.Scripts.HexagonStack stack);
        void ResetSelection();
    }
}