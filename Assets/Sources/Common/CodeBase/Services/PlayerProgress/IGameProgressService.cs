using System;
using Cysharp.Threading.Tasks;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public interface IGameProgressService
    {
        public event Action ProgressLoaded;
        GameProgress GameProgress { get; }
        void ApplyProgress();
        UniTask SavePersistentProgressAsync();
        UniTask LoadProgressAsync();
        UniTask<bool> SavedProgressExists();
        void InitializeNewProgress();
        UniTask SaveControlPointProgressAsync();
        void PersistentProgressToControlPoint();
    }
}