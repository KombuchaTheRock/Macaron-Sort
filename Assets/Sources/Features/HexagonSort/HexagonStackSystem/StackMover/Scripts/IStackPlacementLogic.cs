using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackPlacementLogic
    {
        void PlaceOnGrid(StackGenerator.Scripts.HexagonStack stack, GridCell cell);
        void ReturnToInitialPosition(StackGenerator.Scripts.HexagonStack stack, Vector3 initialPosition);
    }
}