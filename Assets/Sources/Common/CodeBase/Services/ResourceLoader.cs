using UnityEngine;

namespace Sources.Common.CodeBase.Services
{
    public class ResourceLoader : IResourceLoader
    {
        public T LoadAsset<T>(string path) where T : Object => 
            Resources.Load<T>(path);
        
        public T[] LoadAllAssets<T>(string path) where T : Object => 
            Resources.LoadAll<T>(path);
    }
}