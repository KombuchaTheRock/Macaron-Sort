using System;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;

public class BoosterActivator : IDisposable, IBoosterActivator
{
    private BoosterPicker _boosterPicker;
    private IStackMover _stackMover;
    private readonly IStackSpawner _stackSpawner;

    private bool _arrowBoosterActive;
    private bool _reverseBoosterActive;

    public BoosterActivator(IStackMover stackMover, IStackSpawner stackSpawner)
    {
        _stackMover = stackMover;
        _stackSpawner = stackSpawner;
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
        _stackMover.StackPlaced += OnStackPlaced;
        _boosterPicker.BoosterPicked += OnBoosterPicked;
    }

    private void CleanUp()
    {
        _stackMover.StackPlaced -= OnStackPlaced;
        _boosterPicker.BoosterPicked -= OnBoosterPicked;
    }
}