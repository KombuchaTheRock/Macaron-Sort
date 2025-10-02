using System;
using System.Collections;
using System.Collections.Generic;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class StacksPlacer : IDisposable, IStacksSpawner
    {
        private List<HexagonStack> _generatedStacks = new();
        private Transform _stacksRoot;
        private Coroutine _stackGenerateRoutine;

        private readonly IStackMover _stackMover;
        private readonly IStackGenerator _stackGenerator;
        private readonly IHexagonFactory _hexagonFactory;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly HexagonStackConfig _stackConfig;
        private readonly Vector3[] _spawnPositions;
        private readonly float _delayBetweenStacks;

        public StacksPlacer(IStackMover stackMover, IStackGenerator stackGenerator, IStaticDataService staticData,
            IHexagonFactory hexagonFactory, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _hexagonFactory = hexagonFactory;
            _stackGenerator = stackGenerator;
            _stackMover = stackMover;
            _stackConfig = staticData.ForHexagonStack(HexagonStackTemplate.Default);
            _spawnPositions = staticData.GameConfig.LevelConfig.StackSpawnPoints.ToArray();
            _delayBetweenStacks = staticData.GameConfig.StacksSpawnerConfig.DelayBetweenStacks;
            
            SubscribeUpdates();
        }

        public void Dispose() => 
            CleanUp();

        private void SubscribeUpdates() =>
            _stackMover.StackPlaced += OnStackPlaced;

        private void CleanUp() => 
            _stackMover.StackPlaced -= OnStackPlaced;

        private void OnStackPlaced(GridCell cell)
        {
            _generatedStacks.Remove(cell.Stack);

            if (_generatedStacks.Count <= 0)
                SpawnStacks();
        }

        public void SpawnStacks()
        {
            _stacksRoot ??= _hexagonFactory.CreateStacksRoot();

            if (_stackGenerateRoutine != null)
                _coroutineRunner.StopCoroutine(_stackGenerateRoutine);

            _stackGenerateRoutine = _coroutineRunner.StartCoroutine(SpawnStacksRoutine(_spawnPositions,
                _stackConfig,
                _delayBetweenStacks,
                OnStacksGenerated));
        }

        private IEnumerator SpawnStacksRoutine(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks, Action<List<HexagonStack>> onStacksGenerated = null)
        {
            List<HexagonStack> generatedStacks = new();

            foreach (Vector3 position in spawnPositions)
            {
                HexagonStack stack = _stackGenerator.GenerateStack(position, stackConfig);
                generatedStacks.Add(stack);

                yield return new WaitForSeconds(delayBetweenStacks);
            }

            onStacksGenerated?.Invoke(generatedStacks);
        }

        private void OnStacksGenerated(List<HexagonStack> stacks) =>
            _generatedStacks = stacks;
    }
}