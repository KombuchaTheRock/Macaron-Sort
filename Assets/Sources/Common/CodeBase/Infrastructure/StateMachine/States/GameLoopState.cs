using Sources.Common.CodeBase.Services;
using Sources.Common.CodeBase.Services.PlayerProgress;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly IGameFactory _factory;
        private readonly IStaticDataService _staticData;
        private readonly IPlayerLevel _playerLevel;
        private readonly IStackGenerator _stackGenerator;

        private int _stacksAmount;
        private StackMover _stackMover;
        private MergeSystem _mergeSystem;

        public GameLoopState(IGameFactory factory, IStackGenerator stackGenerator, 
            IStaticDataService staticData, IPlayerLevel playerLevel)
        {
            _stackGenerator = stackGenerator;
            _factory = factory;
            _staticData = staticData;
            _playerLevel = playerLevel;
        }

        public void Enter()
        {
            _stackMover = _factory.StackMover;
            _mergeSystem = _factory.MergeSystem;

            _stackMover.StackPlaced += OnStackPlaced;
            _stackMover.DragStarted += OnDragStarted;
            _stackMover.DragFinished += OnDragFinished;

            _mergeSystem.MergeStarted += OnMergeStarted;
            _mergeSystem.MergeFinished += OnMergeFinished;
            _mergeSystem.StackCompleted += OnStackCompleted;

            GenerateStacks();
            _stacksAmount = _staticData.GameConfig.LevelConfig.StackSpawnPoints.Count;
        }

        private void OnStackCompleted(int score) => 
            _playerLevel.AddScore(score);

        public void Exit()
        {
            _stackMover.StackPlaced -= OnStackPlaced;
            _stackMover.DragStarted -= OnDragStarted;
            _stackMover.DragFinished -= OnDragFinished;

            _mergeSystem.MergeStarted -= OnMergeStarted;
            _mergeSystem.MergeFinished -= OnMergeFinished;
        }

        private void OnMergeStarted() =>
            UpdateGridRotationEnabled();

        private void OnMergeFinished() =>
            UpdateGridRotationEnabled();

        private void OnDragFinished() =>
            UpdateGridRotationEnabled();

        private void OnDragStarted() =>
            UpdateGridRotationEnabled();

        private void OnStackPlaced(GridCell cell)
        {
            if (_stackMover.StacksOnGridCount >= _stacksAmount)
            {
                GenerateStacks();
                _stackMover.ResetStacksOnGridCount();
            }
        }

        private void UpdateGridRotationEnabled()
        {
            if (_stackMover.IsDragging || _mergeSystem.IsMerging)
                _factory.GridRotator.enabled = false;
            else
                _factory.GridRotator.enabled = true;
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
}