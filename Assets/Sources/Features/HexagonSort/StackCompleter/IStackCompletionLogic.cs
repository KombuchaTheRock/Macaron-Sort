using System;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;

public interface IStackCompletionLogic
{
    event Action<int> StackCompleted;
    event Action DeleteAnimationCompleted;
    
    void CompleteStack(HexagonStack stack, GridCell gridCell);
}