using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.SaveService;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class GameProgressService : IGameProgressService
    {
        public GameProgress GameProgress { get; private set; }
        public event Action ProgressLoaded;

        private readonly ISaveSystem _saveSystem;
        private readonly IGameFactory _gameFactory;
        private readonly ISaveDataFactory _saveDataFactory;

        private CancellationTokenSource _playerSaveCancellationToken;
        private CancellationTokenSource _worldSaveCancellationToken;

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

        public async UniTask SaveProgressAsync()
        {
            CancelCurrentSaves();
            ResetCancellationTokens();

            List<UniTask> tasks = new();

            if (await _saveSystem.ExistsAsync<PlayerData>() == false)
                tasks.Add(SaveDataAsync(GameProgress.PlayerData, _playerSaveCancellationToken));

            if (await _saveSystem.ExistsAsync<WorldData>() == false)
                tasks.Add(SaveDataAsync(GameProgress.WorldData, _worldSaveCancellationToken));

            await UniTask.WhenAll(tasks);
        }

        public async UniTask SaveControlPoint()
        {
            CancelCurrentSaves();
            ResetCancellationTokens();

            List<UniTask> tasks = new();

            if (await _saveSystem.ExistsAsync<PlayerData>() == false)
                tasks.Add(SaveDataAsync(GameProgress.PlayerData, _playerSaveCancellationToken));

            if (await _saveSystem.ExistsAsync<WorldData>() == false)
                tasks.Add(SaveDataAsync(GameProgress.WorldData, _worldSaveCancellationToken));

            await UniTask.WhenAll(tasks);
        }

        public async UniTask LoadProgressAsync()
        {
            PlayerData playerData = await _saveSystem.LoadAsync<PlayerData>();
            WorldData worldData = await _saveSystem.LoadAsync<WorldData>();

            GameProgress = new GameProgress(playerData, worldData);

            ProgressLoaded?.Invoke();
        }

        public async UniTask<bool> SavedProgressExists()
        {
            bool playerDataExists = await _saveSystem.ExistsAsync<PlayerData>();
            bool worldDataExists = await _saveSystem.ExistsAsync<WorldData>();

            return playerDataExists && worldDataExists;
        }

        public void InitializeNewProgress()
        {
            PlayerData playerData = _saveDataFactory.CreateNewPlayerData();
            WorldData worldData = _saveDataFactory.CreateNewWorldData();

            GameProgress = new GameProgress(playerData, worldData);
        }

        private void CancelCurrentSaves()
        {
            _playerSaveCancellationToken?.Cancel();
            _worldSaveCancellationToken?.Cancel();
        }

        private void ResetCancellationTokens()
        {
            _playerSaveCancellationToken = new CancellationTokenSource();
            _worldSaveCancellationToken = new CancellationTokenSource();
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
                UnityEngine.Debug.LogWarning($"Сохранение {typeof(T).Name} отменено (новые изменения)");
            }
        }
    }
}