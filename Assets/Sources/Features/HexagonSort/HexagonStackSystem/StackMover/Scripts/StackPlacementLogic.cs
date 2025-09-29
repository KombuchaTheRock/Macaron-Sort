using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackPlacementLogic : IStackPlacementLogic, IInitializable
    {
        private readonly IStaticDataService _staticData;
        private StackMoverConfig _config;
        
        public StackPlacementLogic(IStaticDataService staticData) => 
            _staticData = staticData;

        public void Initialize() => 
            _config = _staticData.GameConfig.StackMoverConfig;

        public void PlaceOnGrid(HexagonStack stack, GridCell cell)
        {
            cell.SetStack(stack);
            cell.DisableHighlight();
            stack.DisableMovement();
        
            stack.transform.position = cell.transform.position + Vector3.up * _config.PlaceOffsetAboveGrid;
            stack.transform.parent = cell.transform;
        }

        public void ReturnToInitialPosition(HexagonStack stack, Vector3 initialPosition) => 
            stack.MoveToTarget(initialPosition, _config.ToInitialPositionSpeed);
    }
}