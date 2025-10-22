using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.PlayerProgress;

namespace Sources.Common.CodeBase.Services.SaveService
{
    public interface ISaveSystem
    {
        UniTask SaveAsync<TData>(TData data) where TData : ISaveData;
        UniTask<TData> LoadAsync<TData>() where TData : ISaveData;
        UniTask<bool> ExistsAsync<TData>() where TData : ISaveData;
    }
}