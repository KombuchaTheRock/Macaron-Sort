using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public interface IResourceLoader
    {
        T LoadAsset<T>(string path) where T : Object;
        T[] LoadAllAssets<T>(string path) where T : Object;
    }
}