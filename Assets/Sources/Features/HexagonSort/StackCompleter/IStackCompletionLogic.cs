using System;
using System.Collections;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;

public interface IStackCompletionLogic
{
    event Action<int> StackCompleted;
    event Action DeleteAnimationCompleted;
    
    IEnumerator CompleteStackRoutine(HexagonStack stack, GridCell gridCell);
}