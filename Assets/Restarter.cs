using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;
using UnityEngine;
using Zenject;

public class Restarter : MonoBehaviour
{
    private IGameFactory _factory;
    private IStackGenerator _stackGenerator;
    private IStaticDataService _staticData;

    [Inject]
    private void Construct(IGameFactory factory, IStackGenerator stackGenerator, IStaticDataService staticData)
    {
        _staticData = staticData;
        _stackGenerator = stackGenerator;
        _factory = factory;
    }

    public void Restart()
    {
        foreach (GridCell gridCell in _factory.GridCells)
            gridCell.RemoveStack();

        foreach (HexagonStack stack in _factory.Stacks)
            Destroy(stack.gameObject);

        _factory.Stacks.Clear();
        
        GenerateStacks();
    }

    private void GenerateStacks()
    {
        HexagonStackConfig stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
        Vector3[] stackSpawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();

        _stackGenerator.GenerateStacks(stackSpawnPositions,
            stackConfig.MinStackSize,
            stackConfig.MaxStackSize,
            stackConfig.HexagonHeight,
            stackConfig.Colors,
            0.2f);
    }
}