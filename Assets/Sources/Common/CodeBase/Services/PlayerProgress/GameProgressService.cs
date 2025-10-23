using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.Factories.GameFactory;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Common.CodeBase.Services.SaveService;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class GameProgressService : IGameProgressService
    {
        public GameProgress GameProgress { get; private set; }

        public event Action ProgressLoaded;

        private readonly ISaveSystem _saveSystem;
        private readonly IGameFactory _gameFactory;
        private readonly ISaveDataFactory _saveDataFactory;

        private CancellationTokenSource _persistentSaveCancellationToken;
        private CancellationTokenSource _controlPointSaveCancellationToken;

        public GameProgressService(ISaveSystem saveSystem, IGameFactory gameFactory, ISaveDataFactory saveDataFactory)
        {
            _saveSystem = saveSystem;
            _gameFactory = gameFactory;
            _saveDataFactory = saveDataFactory;
        }

        public void ApplyProgress()
        {
            foreach (IProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.ApplyProgress(GameProgress);
        }

        public async UniTask SavePersistentProgressAsync()
        {
            ResetCancellationToken(ref _persistentSaveCancellationToken);
            await SaveDataAsync(GameProgress.PersistentProgressData, _persistentSaveCancellationToken);
        }

        public async UniTask SaveControlPointProgressAsync()
        {
            ResetCancellationToken(ref _controlPointSaveCancellationToken);
            await SaveDataAsync(GameProgress.ControlPointProgressData, _controlPointSaveCancellationToken);
        }

        public async UniTask LoadProgressAsync()
        {
            PersistentProgressData persistentProgressData = await _saveSystem.LoadAsync<PersistentProgressData>();
            ControlPointProgressData controlPointProgressData = await _saveSystem.LoadAsync<ControlPointProgressData>();

            GameProgress = new GameProgress(persistentProgressData, controlPointProgressData);

            ProgressLoaded?.Invoke();
        }

        public void PersistentProgressToControlPoint()
        {
            ControlPointProgressData controlPointProgressData = GameProgress.ControlPointProgressData;
            PersistentProgressData persistentProgressData = ResetPersistentTo(controlPointProgressData);

            GameProgress = new GameProgress(persistentProgressData, controlPointProgressData);

            ProgressLoaded?.Invoke();
        }

        public async UniTask<bool> SavedProgressExists()
        {
            bool persistentProgressDataExists = await _saveSystem.ExistsAsync<PersistentProgressData>();
            bool controlPointProgressDataExists = await _saveSystem.ExistsAsync<ControlPointProgressData>();

            return persistentProgressDataExists && controlPointProgressDataExists;
        }

        public void InitializeNewProgress()
        {
            PersistentProgressData persistentProgressData = _saveDataFactory.CreatePersistentProgressData();
            ControlPointProgressData controlPointProgressData = _saveDataFactory.CreateControlPointProgressData();

            GameProgress = new GameProgress(persistentProgressData, controlPointProgressData);
            ProgressLoaded?.Invoke();
        }

        private PersistentProgressData ResetPersistentTo(ControlPointProgressData controlPointProgressData)
        {
            int level = controlPointProgressData.PlayerData.Level;
            int score = controlPointProgressData.PlayerData.Score;
            IReadOnlyList<BoosterCount> boosters = controlPointProgressData.PlayerData.Boosters;

            PlayerData playerData = new(score, level, boosters.ToList());

            List<PlacedStackData> stacksOnGrid = controlPointProgressData.WorldData.StacksData.StacksOnGrid.ToList();
            List<FreeStackDataData> freeStacks = controlPointProgressData.WorldData.StacksData.FreeStacks.ToList();
            List<CellData> cellData = controlPointProgressData.WorldData.GridData.Cells.ToList();
            
            StacksData stacksData = new(stacksOnGrid, freeStacks);
            GridData gridData = new(cellData);
            
            WorldData worldData = new(stacksData, gridData);

            PersistentProgressData persistentProgressData =
                new(playerData, worldData);

            return persistentProgressData;
        }

        private void ResetCancellationToken(ref CancellationTokenSource tokenSource)
        {
            tokenSource?.Cancel();
            tokenSource = new CancellationTokenSource();
        }

        private async UniTask SaveDataAsync<T>(T data, CancellationTokenSource cancellationToken) where T : ISaveData
        {
            try
            {
                await _saveSystem.SaveAsync(data)
                    .AttachExternalCancellation(cancellationToken.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"Saving {typeof(T).Name} cancelled (new changes)");
            }
        }
    }
}