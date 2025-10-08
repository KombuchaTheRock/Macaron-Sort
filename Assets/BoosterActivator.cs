using System;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.StackSelector;

public class BoosterActivator : IDisposable, IBoosterActivator
{
    private bool _arrowBoosterActive;
    private bool _reverseBoosterActive;
    private bool _rocketBoosterActive;

    private BoosterPicker _boosterPicker;
    private GridRotator _gridRotator;

    private readonly IStackMover _stackMover;
    private readonly IStackSpawner _stackSpawner;
    private readonly IStackCompleter _stackCompleter;
    private readonly IBoosterCounter _boosterCounter;

    public BoosterActivator(IStackMover stackMover, IStackSpawner stackSpawner, IStackCompleter stackCompleter,
        IBoosterCounter boosterCounter)
    {
        _stackMover = stackMover;
        _stackSpawner = stackSpawner;
        _stackCompleter = stackCompleter;
        _boosterCounter = boosterCounter;
    }

    public void Initialize(BoosterPicker boosterPicker, HexagonGrid hexagonGrid)
    {
        _gridRotator = hexagonGrid.GetComponent<GridRotator>();
        _boosterPicker = boosterPicker;

        _arrowBoosterActive = false;
        _reverseBoosterActive = false;
        _rocketBoosterActive = false;

        SubscribeUpdates();
    }

    public void Dispose() =>
        CleanUp();

    public void Reset()
    {
        _stackSpawner.StopSpawn();
        _stackMover.Activate();
        _stackMover.DeactivateOnGridSelection();

        _stackCompleter.Deactivate();

        _arrowBoosterActive = false;
        _reverseBoosterActive = false;
        _rocketBoosterActive = false;

        _gridRotator.enabled = true;
        //EnableButtons();
    }

    private void OnBoosterPicked(BoosterType boosterType)
    {
        //_boosterPicker.DisableInteractable();

        switch (boosterType)
        {
            case BoosterType.RocketBooster:
                if (_rocketBoosterActive || HasBooster(boosterType) == false)
                    break;
                
                ActivateRocketBooster();
                break;
            case BoosterType.ArrowBooster:
                if (_arrowBoosterActive || HasBooster(boosterType) == false)
                    break;
                
                ActivateArrowBooster();
                break;
            case BoosterType.ReverseBooster:
                if (_reverseBoosterActive || HasBooster(boosterType) == false)
                    break;
                
                ActivateReverseBooster();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
        }
    }

    private bool HasBooster(BoosterType boosterType) => 
        _boosterCounter.BoostersCount[boosterType] > 0;

    private void ActivateRocketBooster()
    {
        _rocketBoosterActive = true;

        _gridRotator.enabled = false;
        _stackCompleter.Activate();
        _stackMover.Deactivate();
    }

    private void ActivateReverseBooster()
    {
        _reverseBoosterActive = true;
        _stackSpawner.SpawnNewStacks();
    }

    private void ActivateArrowBooster()
    {
        _arrowBoosterActive = true;
        _gridRotator.enabled = false;
        _stackMover.ActivateOnGridSelection();
    }

    private void OnStackPlaced(GridCell gridCell)
    {
        if (_arrowBoosterActive == false)
            return;

        FinishArrowBooster();
        
        // EnableButtons();
    }

    private void EnableButtons()
    {
     //   _boosterPicker.EnableInteractable();
        _boosterPicker.UpdateButtonEnabled();
    }

    private void OnStackCompleted(int score)
    {
        if (_rocketBoosterActive == false)
            return;

        _gridRotator.enabled = true;
        FinishRocketBooster();
        
        // EnableButtons();
    }

    private void OnStacksSpawned()
    {
        if (_reverseBoosterActive == false)
            return;

        _reverseBoosterActive = false;
        _boosterCounter.RemoveBooster(BoosterType.ReverseBooster);
        // EnableButtons();
    }

    private void FinishArrowBooster()
    {
        _stackMover.DeactivateOnGridSelection();
        _stackMover.InitialCell.SetStack(null);

        _gridRotator.enabled = true;
        _arrowBoosterActive = false;
        _boosterCounter.RemoveBooster(BoosterType.ArrowBooster);
    }

    private void FinishRocketBooster()
    {
        _stackMover.Activate();
        _stackCompleter.Deactivate();

        _rocketBoosterActive = false;
        _boosterCounter.RemoveBooster(BoosterType.RocketBooster);
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
        _boosterPicker.BoosterPicked -= OnBoosterPicked;
    }
}