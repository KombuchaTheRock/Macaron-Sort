using System.IO;
using Cysharp.Threading.Tasks;
using Sources.Common.CodeBase.Services.PlayerProgress;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.SaveService
{
    public sealed class SaveSystem : ISaveSystem
    {
        private const string FileExtension = "json";
    
        private readonly ISerializer _serializer;
        private readonly string _folderPath;
    
        public SaveSystem(ISerializer serializer)
        {
            _serializer = serializer;
            _folderPath = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
        }

        public async UniTask SaveAsync<TData>(TData data) where TData : ISaveData
        {
            string dataKey = SaveDataKeys.GetKey<TData>();
            string serializedData = await _serializer.SerializeAsync(data);

            await WriteToDataStorageAsync(dataKey, serializedData);
        }
    
        public async UniTask<TData> LoadAsync<TData>() where TData : ISaveData
        {
            string dataKey = SaveDataKeys.GetKey<TData>();
            string serializedData = await ReadFromDataStorageAsync(dataKey);

            return await _serializer.DeserializeAsync<TData>(serializedData);
        }
    
        public UniTask<bool> ExistsAsync<TData>() where TData : ISaveData
        {
            string dataKey = SaveDataKeys.GetKey<TData>();
            string filePath = GetFilePath(dataKey);
            bool exists = File.Exists(filePath);
        
            return UniTask.FromResult(exists);
        }
    
        private async UniTask WriteToDataStorageAsync(string dataKey, string serializedData)
        {
            string filePath = GetFilePath(dataKey);
            await File.WriteAllTextAsync(filePath, serializedData);
        }
    
        private async UniTask<string> ReadFromDataStorageAsync(string dataKey)
        {
            string filePath = GetFilePath(dataKey);
            return await File.ReadAllTextAsync(filePath);
        }
    
        private string GetFilePath(string key) =>
            Path.Combine(_folderPath, key) + "." + FileExtension;
    }
}
