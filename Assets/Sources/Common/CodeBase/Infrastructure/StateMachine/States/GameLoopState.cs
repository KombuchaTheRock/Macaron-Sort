using Sources.Common.CodeBase.Services;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;
        private readonly IStackGenerator _stackGenerator;

        private int _stacksAmount;
        private int _stacksOnGridCount;
        private StackMover _stackMover;

        public GameLoopState(IGameFactory factory, IStackGenerator stackGenerator, IStaticDataService staticData)
        {
            _stackGenerator = stackGenerator;
            _factory = factory;
            _staticData = staticData;
        }
        
        public void Dispose() =>
            Exit();

        public void Enter()
        {
            _stackMover = _factory.StackMover;

            _stackMover.StackPlaced += OnStackPlaced;
            _stackMover.DragStarted += OnDragStarted;
            _stackMover.DragFinished += OnDragFinished;

            GenerateStacks();
            _stacksAmount = _staticData.GameConfig.LevelConfig.StackSpawnPoints.Count;
        }

        public void Exit()
        {
            _stackMover.StackPlaced -= OnStackPlaced;
            _stackMover.DragStarted -= OnDragStarted;
            _stackMover.DragFinished -= OnDragFinished;
        }

        private void OnDragFinished() =>
            _factory.GridRotator.enabled = true;

        private void OnDragStarted() =>
            _factory.GridRotator.enabled = false;

        private void OnStackPlaced(Vector2Int position)
        {
            _stacksOnGridCount++;

            if (_stacksOnGridCount >= _stacksAmount)
            {
                GenerateStacks();
                _stacksOnGridCount = 0;
            }
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
}