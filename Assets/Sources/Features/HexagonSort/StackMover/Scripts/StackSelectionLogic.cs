using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.StackMover.Scripts
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

        public HexagonStack SelectedStack { get; private set; }

        public bool TrySelectStack(Ray ray, out HexagonStack stack)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _config.MaxRaycastDistance, _stackLayerMask))
            {
                stack = hit.collider.GetComponentInParent<HexagonStack>();
                
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