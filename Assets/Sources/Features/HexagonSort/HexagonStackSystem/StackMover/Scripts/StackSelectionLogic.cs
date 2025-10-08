using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts
{
    public class StackSelectionLogic : IStackSelectionLogic, IInitializable
    {
        private float _maxRaycastDistance;
        private LayerMask _stackLayerMask;
        private LayerMask _gridLayerMask;
        private StackMoverConfig _config;

        private readonly IStaticDataService _staticData;

        public StackSelectionLogic(IStaticDataService staticData) =>
            _staticData = staticData;

        public void Initialize()
        {
            _config = _staticData.GameConfig.StackMoverConfig;
            _stackLayerMask = 1 << _config.StackLayer;
            _gridLayerMask = 1 << _config.GridLayer;
        }

        public HexagonStack SelectedStack { get; private set; }

        public bool TrySelectFreeStack(Ray ray, out HexagonStack stack)
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

        public bool TrySelectStackOnGrid(Ray ray, out HexagonStack stack, out GridCell gridCell)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, _config.MaxRaycastDistance, _stackLayerMask))
            {
                stack = hit.collider.GetComponentInParent<HexagonStack>();

                Debug.Log(stack.CanMove);
                
                if (stack.CanMove == false)
                {
                    SelectedStack = stack;
                    gridCell = GetCellUnderStack(stack);

                    return true;
                }
            }

            stack = null;
            gridCell = null;

            return false;
        }

        public void ResetSelection() =>
            SelectedStack = null;

        private GridCell GetCellUnderStack(HexagonStack stack)
        {
            Ray checkRay = new(stack.transform.position, Vector3.down);
            Physics.Raycast(checkRay, out RaycastHit hit, _config.MaxRaycastDistance, _gridLayerMask);

            return hit.collider.GetComponent<GridCell>();
        }
    }
}