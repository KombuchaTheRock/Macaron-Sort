using Cysharp.Threading.Tasks;

namespace Sources.Common.CodeBase.Services.SaveService
{
    public interface ISerializer
    {
        UniTask<string> SerializeAsync<TData>(TData data);
        UniTask<TData> DeserializeAsync<TData>(string serializedData);
    }
}
