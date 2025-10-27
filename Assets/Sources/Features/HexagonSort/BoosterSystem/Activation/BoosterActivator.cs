using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.BoosterSystem.Boosters;
using Sources.Features.HexagonSort.BoosterSystem.Counter;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.StackCompleter;
using UnityEngine;

namespace Sources.Features.HexagonSort.BoosterSystem.Activation
{
    public class BoosterActivator : IDisposable, IBoosterActivator
    {
        private Dictionary<BoosterType, IBooster> _boosters;

        private BoosterPicker _boosterPicker;
        private GridRotator _gridRotator;

        private readonly IStackMover _stackMover;
        private readonly IStackSpawner _stackSpawner;
        private readonly IStackCompleter _stackCompleter;
        private readonly IWindowService _windowService;
        private readonly BoosterContext _context;

        private IBooster _activeBooster;
        private BoosterWindow _currentBoosterWindow;
        private CameraViewSwitcher _cameraViewSwitcher;

        public BoosterActivator(IStackMover stackMover, IStackSpawner stackSpawner, IStackCompleter stackCompleter,
            IBoosterCounter boosterCounter, IWindowService windowService)
        {
            _context = new BoosterContext
            {
                StackMover = stackMover,
                StackSpawner = stackSpawner,
                StackCompleter = stackCompleter,
                BoosterCounter = boosterCounter
            };

            _stackMover = stackMover;
            _stackSpawner = stackSpawner;
            _stackCompleter = stackCompleter;
            _windowService = windowService;

            _boosters = new Dictionary<BoosterType, IBooster>
            {
                [BoosterType.RocketBooster] = new RocketBooster(_context),
                [BoosterType.ArrowBooster] = new ArrowBooster(_context),
                [BoosterType.ReverseBooster] = new ReverseBooster(_context)
            };
        }

        public void Initialize(BoosterPicker boosterPicker, HexagonGrid hexagonGrid)
        {
            _gridRotator = hexagonGrid.GetComponent<GridRotator>();
            _boosterPicker = boosterPicker;

            _cameraViewSwitcher = Camera.main.GetComponent<CameraViewSwitcher>();
            _context.GridRotator = _gridRotator;

            SubscribeUpdates();
        }

        public void Dispose() =>
            CleanUp();

        public void Reset()
        {
            foreach (IBooster booster in _boosters.Values) 
                booster.TryFinish();
        
            _stackSpawner.StopSpawn();
        }

        private void OnBoosterPicked(BoosterType boosterType)
        {
            if (_activeBooster != null)
                return;

            if (_boosters[boosterType].TryActivate())
            {
                _activeBooster = _boosters[boosterType];
                
                _cameraViewSwitcher.ToBoosterTransform();
                
                switch (boosterType)
                {
                    case BoosterType.RocketBooster:
                        _currentBoosterWindow = (BoosterWindow)_windowService.Open(WindowID.RocketBooster);
                        _currentBoosterWindow.CloseButtonClicked += OnCloseButtonClicked;
                        break;
                    case BoosterType.ArrowBooster:
                        _currentBoosterWindow = (BoosterWindow)_windowService.Open(WindowID.ArrowBooster);
                        _currentBoosterWindow.CloseButtonClicked += OnCloseButtonClicked;
                        break;
                }
            }
        }

        private void OnCloseButtonClicked()
        {
            if (_activeBooster is ICancellableBooster cancellableBooster)
            {
                cancellableBooster.Cancel();
                _activeBooster = null;
                _currentBoosterWindow.CloseButtonClicked -= OnCloseButtonClicked;
                _currentBoosterWindow = null;
                
                _cameraViewSwitcher.ToDefaultTransform();
            }
        }

        private void OnStackPlaced(GridCell gridCell) => 
            FinishBooster(BoosterType.ArrowBooster);

        private void OnStackCompleted(HexagonStackScore stackScore) => 
            FinishBooster(BoosterType.RocketBooster);

        private void OnStacksSpawned() => 
            FinishBooster(BoosterType.ReverseBooster);

        private void FinishBooster(BoosterType boosterType)
        {
            if (_currentBoosterWindow != null) 
                _currentBoosterWindow.CloseWindow();
            
            _boosters[boosterType].TryFinish();
            
            _activeBooster = null;
            _currentBoosterWindow = null;
            
            _boosterPicker.LoadButtonsInteractable();
            
            _cameraViewSwitcher.ToDefaultTransform();
        }

        private void SubscribeUpdates()
        {
            _stackCompleter.StackCompleted += OnStackCompleted;
            _stackMover.StackPlaced += OnStackPlaced;
            _stackSpawner.StacksSpawned += OnStacksSpawned;

            _boosterPicker.BoosterPicked += OnBoosterPicked;
        }

        private void CleanUp()
        {
            _stackCompleter.StackCompleted -= OnStackCompleted;
            _stackMover.StackPlaced -= OnStackPlaced;
            _stackSpawner.StacksSpawned -= OnStacksSpawned;
        
            _boosterPicker.BoosterPicked -= OnBoosterPicked;

            if (_currentBoosterWindow != null) 
                _currentBoosterWindow.CloseButtonClicked -= OnCloseButtonClicked;
        }
    }
}