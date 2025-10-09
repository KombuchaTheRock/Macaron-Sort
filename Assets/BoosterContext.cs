using Sources.Common.CodeBase.Infrastructure.StateMachine.States;
using Sources.Features.HexagonSort.GridSystem.GridRotator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.StackSelector;

public class BoosterContext
{
    public GridRotator GridRotator { get; set; }
    public IStackMover StackMover { get; set; }
    public IStackSpawner StackSpawner { get; set; }
    public IStackCompleter StackCompleter { get; set; }
    public IBoosterCounter BoosterCounter { get; set; }
}