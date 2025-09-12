using DG.Tweening;
using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using Sources.Features.HexagonSort.StackMover.Scripts;
using UnityEngine;
using Zenject;

public class SimpleLevelRestarter : MonoBehaviour
{
    [SerializeField] private StackMover _stackMover;
    private IGameFactory _factory;

    [Inject]
    public void Construct(IGameFactory factory)
    {
        _factory = factory;
    }

    public void RestartLevel()
    {
        DOTween.Kill(_stackMover.gameObject);
        foreach (HexagonStack stack in _factory.Stacks)
        {
            Destroy(stack.gameObject);
        }

        _factory.Stacks.Clear();
        _factory.StackGenerator.GenerateStacks();
    }
}