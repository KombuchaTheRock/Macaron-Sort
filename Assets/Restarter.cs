using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using TMPro;
using UnityEngine;
using Zenject;

public class Restarter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugText;
    private IGameFactory _factory;
    private IStackGenerator _stackGenerator;
    private IStaticDataService _staticData;
    private IHexagonFactory _hexagonFactory;

    [Inject]
    private void Construct(IGameFactory factory, IHexagonFactory hexagonFactory, IStackGenerator stackGenerator,
        IStaticDataService staticData)
    {
        _hexagonFactory = hexagonFactory;
        _staticData = staticData;
        _stackGenerator = stackGenerator;
        _factory = factory;
    }

    public void Restart()
    {
        if (_factory.StackMover.IsDragging || _factory.MergeSystem.IsMerging)
            return;

        foreach (GridCell gridCell in _factory.GridCells)
            gridCell.RemoveStack();

        foreach (HexagonStack stack in _hexagonFactory.Stacks)
            if (stack != null)
                Destroy(stack.gameObject);

        _hexagonFactory.Stacks.Clear();

        GenerateStacks();
    }

    private void GenerateStacks()
    {
        HexagonStackConfig stackConfig = _staticData.ForHexagonStack(HexagonStackTemplate.Default);
        Vector3[] stackSpawnPositions = _staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();

        _stackGenerator.GenerateStacks(stackSpawnPositions,
            stackConfig,
            0.2f);
    }
}