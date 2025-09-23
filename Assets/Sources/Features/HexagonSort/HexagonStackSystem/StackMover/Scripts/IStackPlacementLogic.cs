using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public interface IStackPlacementLogic
    {
        void PlaceOnGrid(HexagonStack stack, GridCell cell);
        void ReturnToInitialPosition(HexagonStack stack, Vector3 initialPosition);
    }
}