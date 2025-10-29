using Sources.Features.HexagonSort.BoosterSystem.Activation;
using UnityEngine;

namespace Sources.Features.HexagonSort.BoosterSystem.Boosters
{
    public class ArrowBooster : ICancellableBooster
    {
        private readonly BoosterContext _context;
        private CameraViewSwitcher _cameraViewSwitcher;

        public BoosterType Type => BoosterType.ArrowBooster;
        public bool IsActive { get; private set; }

        public ArrowBooster(BoosterContext context)
        {
            _context = context;
        }

        public bool TryActivate()
        {
            if (CanActivate())
            {
                if (_cameraViewSwitcher == null) 
                    _cameraViewSwitcher = Camera.main.GetComponent<CameraViewSwitcher>();
                
                _cameraViewSwitcher.ToBoosterTransform();
                _context.GridRotator.enabled = false;
                _context.StackMover.ActivateOnGridSelection();
                _context.StackSpawner.HideGeneratedStacks();
                
                IsActive = true;

                return true;
            }

            return false;
        }

        public bool TryFinish()
        {
            if (IsActive)
            {
                _cameraViewSwitcher.ToDefaultTransform();
                _context.StackMover.DeactivateOnGridSelection();
                _context.StackSpawner.ShowGeneratedStacks();
                //_context.StackMover.InitialCell.FreeCell();

                _context.GridRotator.enabled = true;

                IsActive = false;
                _context.BoosterCounter.RemoveBooster(Type);
            
                return true;
            }
        
            return false;
        }

        private bool CanActivate()
        {
            return _context.BoosterCounter.BoostersCount[Type] > 0
                   && IsActive == false;
        }

        public void Cancel()
        {
            if (IsActive)
            {
                _cameraViewSwitcher.ToDefaultTransform();
                _context.GridRotator.enabled = true;
                _context.StackMover.DeactivateOnGridSelection();
                _context.StackSpawner.ShowGeneratedStacks();

                IsActive = false;
            }
        }
    }
}