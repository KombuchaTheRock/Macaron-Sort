using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public interface IStackPlacementLogic
    {
        void PlaceOnGrid(HexagonStack stack, GridCell cell);
        void ReturnToInitialPosition(HexagonStack stack, Vector3 initialPosition);
    }
}