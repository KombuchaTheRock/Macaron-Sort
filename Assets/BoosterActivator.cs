using System;
using System.Collections.Generic;
using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.StackSelector;

public class BoosterActivator : IDisposable, IBoosterActivator
{
    private Dictionary<BoosterType, IBooster> _boosters;

    private BoosterPicker _boosterPicker;
    private GridRotator _gridRotator;

    private readonly IStackMover _stackMover;
    private readonly IStackSpawner _stackSpawner;
    private readonly IStackCompleter _stackCompleter;
    private readonly BoosterContext _context;

    public BoosterActivator(IStackMover stackMover, IStackSpawner stackSpawner, IStackCompleter stackCompleter,
        IBoosterCounter boosterCounter)
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

        _context.GridRotator = _gridRotator;

        SubscribeUpdates();
    }

    public void Dispose() =>
        CleanUp();

    public void Reset()
    {
        foreach (IBooster booster in _boosters.Values) 
            booster.TryDeactivate();
        
        _stackSpawner.StopSpawn();
    }

    private void OnBoosterPicked(BoosterType boosterType) => 
        _boosters[boosterType].TryActivate();
   
    private void OnStackPlaced(GridCell gridCell) => 
        _boosters[BoosterType.ArrowBooster].TryDeactivate();

    private void OnStackCompleted(int score) => 
        _boosters[BoosterType.RocketBooster].TryDeactivate();

    private void OnStacksSpawned() => 
        _boosters[BoosterType.ReverseBooster].TryDeactivate();

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
    }
}