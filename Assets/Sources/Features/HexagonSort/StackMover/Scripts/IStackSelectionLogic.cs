using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public interface IStackSelectionLogic
    {
        bool TrySelectStack(Ray ray, out HexagonStack stack);
        void ResetSelection();
    }
}