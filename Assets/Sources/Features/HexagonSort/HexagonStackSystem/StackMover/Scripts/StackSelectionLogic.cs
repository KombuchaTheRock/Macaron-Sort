using Sources.Common.CodeBase.Services;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackSelectionLogic : IStackSelectionLogic, IInitializable
    {
        private readonly IStaticDataService _staticData;
        private  float _maxRaycastDistance;
        private  LayerMask _stackLayerMask;
        private StackMoverConfig _config;

        public StackSelectionLogic(IStaticDataService staticData) => 
            _staticData = staticData;

        public void Initialize()
        {
            _config = _staticData.GameConfig.StackMoverConfig;
            _stackLayerMask = 1 << _config.StackLayer;
        }

        public StackGenerator.Scripts.HexagonStack SelectedStack { get; private set; }

        public bool TrySelectStack(Ray ray, out StackGenerator.Scripts.HexagonStack stack)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _config.MaxRaycastDistance, _stackLayerMask))
            {
                stack = hit.collider.GetComponentInParent<StackGenerator.Scripts.HexagonStack>();
                
                if (stack.CanMove)
                {
                    SelectedStack = stack;
                    return true;
                }
            }
        
            stack = null;
            return false;
        }

        public void ResetSelection() => 
            SelectedStack = null;
    }
}