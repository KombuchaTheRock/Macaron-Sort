using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sources.Common.CodeBase.Infrastructure.Extensions;
using Sources.Common.CodeBase.Services.Factories.HexagonFactory;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public class StackSpawner : IDisposable, IStackSpawner
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

        public StackSpawner(IStackMover stackMover, IStackGenerator stackGenerator, IStaticDataService staticData,
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

        public void StopSpawn()
        {
            if (_stackGenerateRoutine != null)
            {
                _coroutineRunner.StopCoroutine(_stackGenerateRoutine);
                _stackGenerateRoutine = null;
            }
            
            _generatedStacks.Clear();
        }

        public void Dispose() =>
            CleanUp();

        public void SpawnNewStacks()
        {
            _stacksRoot ??= _hexagonFactory.CreateStacksRoot();

            if (_stackGenerateRoutine != null)
                return;

            _stackGenerateRoutine = _coroutineRunner.StartCoroutine(SpawnStacksRoutine(_spawnPositions,
                _stackConfig,
                _delayBetweenStacks,
                OnStacksGenerated));
        }

        private IEnumerator DeleteGeneratedStacks(float delayBetweenStacks)
        {
            Tween removeAnimation = null;
            foreach (HexagonStack stack in _generatedStacks)
            {
                removeAnimation = stack.ActivateRemoveAnimation(() => Object.Destroy(stack.gameObject), Ease.InQuart);
                yield return new WaitForSeconds(delayBetweenStacks);
            }

            yield return new DOTweenCYInstruction.WaitForCompletion(removeAnimation);

            _generatedStacks.Clear();
        }

        private void SubscribeUpdates() =>
            _stackMover.StackPlaced += OnStackPlaced;

        private void CleanUp() =>
            _stackMover.StackPlaced -= OnStackPlaced;

        private void OnStackPlaced(GridCell cell)
        {
            _generatedStacks.Remove(cell.Stack);

            if (_generatedStacks.Count <= 0)
                SpawnNewStacks();
        }

        private IEnumerator SpawnStacksRoutine(Vector3[] spawnPositions, HexagonStackConfig stackConfig,
            float delayBetweenStacks, Action<List<HexagonStack>> onStacksGenerated = null)
        {
            if (_generatedStacks.Count > 0)
                yield return _coroutineRunner.StartCoroutine(DeleteGeneratedStacks(delayBetweenStacks));

            List<HexagonStack> generatedStacks = new();

            foreach (Vector3 position in spawnPositions)
            {
                HexagonStack stack = _stackGenerator.GenerateStack(position, stackConfig);
                generatedStacks.Add(stack);

                yield return new WaitForSeconds(delayBetweenStacks);
            }

            onStacksGenerated?.Invoke(generatedStacks);
        }

        private void OnStacksGenerated(List<HexagonStack> stacks)
        {
            _stackGenerateRoutine = null;
            _generatedStacks = stacks;
        }
    }
}