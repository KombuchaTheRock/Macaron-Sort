using System;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.StackSelector;
using UnityEngine;

public class BoosterActivator : IDisposable, IBoosterActivator
{
    private BoosterPicker _boosterPicker;
    private IStackMover _stackMover;
    private readonly IStackSpawner _stackSpawner;
    private readonly IStackCompleter _stackCompleter;

    private bool _arrowBoosterActive;
    private bool _reverseBoosterActive;
    private bool _rocketBoosterActive;

    public BoosterActivator(IStackMover stackMover, IStackSpawner stackSpawner, IStackCompleter stackCompleter)
    {
        _stackMover = stackMover;
        _stackSpawner = stackSpawner;
        _stackCompleter = stackCompleter;
    }

    public void Initialize(BoosterPicker boosterPicker)
    {
        _boosterPicker = boosterPicker;
        _arrowBoosterActive = false;

        SubscribeUpdates();
    }

    public void Dispose()
    {
        CleanUp();
    }

    private void OnStackPlaced(GridCell gridCell)
    {
        if (_arrowBoosterActive)
        {
            Debug.Log("Arrow booster deactivated");
            
            _stackMover.DeactivateOnGridSelection();
            _stackMover.InitialCell.SetStack(null);
            
            _arrowBoosterActive = false;
        }
    }

    private void OnBoosterPicked(BoosterType boosterType)
    {
        switch (boosterType)
        {
            case BoosterType.RocketBooster:
                ActivateRocketBooster();
                break;
            case BoosterType.ArrowBooster:
                ActivateArrowBooster();
                break;
            case BoosterType.ReverseBooster:
                ActivateReverseBooster();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(boosterType), boosterType, null);
        }
    }

    private void ActivateRocketBooster()
    {
        _rocketBoosterActive = true;
        
        _stackCompleter.Activate();
        _stackMover.Deactivate();
    }

    private void ActivateReverseBooster()
    {
        _stackSpawner.SpawnNewStacks();
    }

    private void ActivateArrowBooster()
    {
        _arrowBoosterActive = true;
        _stackMover.ActivateOnGridSelection();
    }

    private void SubscribeUpdates()
    {
        _stackCompleter.StackCompleted += OnStackCompleted;
        _stackMover.StackPlaced += OnStackPlaced;
        _boosterPicker.BoosterPicked += OnBoosterPicked;
    }

    private void OnStackCompleted(int score)
    {
        if (_rocketBoosterActive)
        {
            _stackMover.Activate();
            _stackCompleter.Deactivate();
            
            _rocketBoosterActive = false;
        }
    }

    private void CleanUp()
    {
        _stackMover.StackPlaced -= OnStackPlaced;
        _boosterPicker.BoosterPicked -= OnBoosterPicked;
    }
}