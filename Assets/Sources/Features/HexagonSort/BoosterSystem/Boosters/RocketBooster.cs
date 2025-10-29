using Sources.Features.HexagonSort.BoosterSystem.Activation;
using UnityEngine;

namespace Sources.Features.HexagonSort.BoosterSystem.Boosters
{
    public class RocketBooster : ICancellableBooster
    {
        private readonly BoosterContext _context;
        private CameraViewSwitcher _cameraViewSwitcher;

        public BoosterType Type => BoosterType.RocketBooster;
        public bool IsActive { get; private set; }

        public RocketBooster(BoosterContext context)
        {
            _context = context;
        }

        public bool TryActivate()
        {
            if (CanActivate() == false)
                return false;

            if (_cameraViewSwitcher == null) 
                _cameraViewSwitcher = Camera.main.GetComponent<CameraViewSwitcher>();

            _cameraViewSwitcher.ToBoosterTransform();
            _context.GridRotator.enabled = false;
            _context.StackMover.Deactivate();
            _context.StackCompleter.Activate();
            _context.StackSpawner.HideGeneratedStacks();

            IsActive = true;

            return true;
        }

        public bool TryFinish()
        {
            if (IsActive)
            {
                _context.GridRotator.enabled = true;
                _context.StackMover.Activate();
                _context.StackCompleter.Deactivate();
                _context.StackSpawner.ShowGeneratedStacks();
                _cameraViewSwitcher.ToDefaultTransform();

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
                _context.GridRotator.enabled = true;
                _context.StackMover.Activate();
                _context.StackCompleter.Deactivate();
                _context.StackSpawner.ShowGeneratedStacks();
                _cameraViewSwitcher.ToDefaultTransform();

                IsActive = false;
            }
        }
    }
}