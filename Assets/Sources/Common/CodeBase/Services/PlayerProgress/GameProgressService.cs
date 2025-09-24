using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
            Debug.Log("Saving game progress");
            
            CancelPersistentProgressSave();
            ResetPersistentCancellationToken();

            await SaveDataAsync(GameProgress.PersistentProgressData, _persistentSaveCancellationToken);
            
        }

        public async UniTask SaveControlPointProgressAsync()
        {
            CancelControlPointProgressSave();
            ResetControlPointCancellationToken();

            await SaveDataAsync(GameProgress.ControlPointProgressData, _controlPointSaveCancellationToken);
        }

        public async UniTask LoadProgressAsync()
        {
            Debug.Log("Loading game progress");
            
            PersistentProgressData persistentProgressData = await _saveSystem.LoadAsync<PersistentProgressData>();
            ControlPointProgressData controlPointProgressData = await _saveSystem.LoadAsync<ControlPointProgressData>();;

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
            Debug.Log("Initializing new game progress");

            PersistentProgressData persistentProgressData = _saveDataFactory.CreatePersistentProgressData();
            ControlPointProgressData controlPointProgressData = _saveDataFactory.CreateControlPointProgressData();

            GameProgress = new GameProgress(persistentProgressData, controlPointProgressData);
            ProgressLoaded?.Invoke();
        }

        private void CancelControlPointProgressSave() => 
            _controlPointSaveCancellationToken?.Cancel();

        private void CancelPersistentProgressSave() => 
            _persistentSaveCancellationToken?.Cancel();

        private void ResetPersistentCancellationToken() => 
            _persistentSaveCancellationToken = new CancellationTokenSource();

        private void ResetControlPointCancellationToken() => 
            _controlPointSaveCancellationToken = new CancellationTokenSource();

        private async UniTask SaveDataAsync<T>(T data, CancellationTokenSource cancellationToken) where T : ISaveData
        {
            try
            {
                await _saveSystem.SaveAsync(data)
                    .AttachExternalCancellation(cancellationToken.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning($"Сохранение {typeof(T).Name} отменено (новые изменения)");
            }
        }
    }
}