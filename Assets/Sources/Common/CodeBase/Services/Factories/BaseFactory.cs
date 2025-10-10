using System.Collections.Generic;
using Sources.Common.CodeBase.Services.ResourceLoader;
using Sources.Common.CodeBase.Services.Settings;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.Factories
{
    public abstract class BaseFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IResourceLoader _resourceLoader;

        protected BaseFactory(IInstantiator instantiator, IResourceLoader resourceLoader)
        {
            _instantiator = instantiator;
            _resourceLoader = resourceLoader;
        }


        protected GameObject Instantiate(string assetPath, Vector3 at, Transform parent = null)
        {
            GameObject prefab = _resourceLoader.LoadAsset<GameObject>(assetPath);
            GameObject instantiatedPrefab = _instantiator.InstantiatePrefab(prefab, at, Quaternion.identity, parent);

            return instantiatedPrefab;
        }

        protected GameObject Instantiate(GameObject prefab, Transform parent)
        {
            GameObject instantiatedPrefab = _instantiator.InstantiatePrefab(prefab, parent);

            return instantiatedPrefab;
        }
        
        protected T Instantiate<T>(GameObject prefab, Transform parent)
        {
            T instantiatedPrefab = _instantiator.InstantiatePrefabForComponent<T>(prefab, parent);

            return instantiatedPrefab;
        }

        protected T Instantiate<T>(string assetPath, Vector3 at, Transform parent = null) where T : Component
        {
            GameObject prefab = _resourceLoader.LoadAsset<GameObject>(assetPath);
            T instantiatedPrefab =
                _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);

            return instantiatedPrefab;
        }

        protected T Instantiate<T>(GameObject prefab, Vector3 at, Transform parent = null) where T : Component
        {
            T instantiatedPrefab =
                _instantiator.InstantiatePrefabForComponent<T>(prefab, at, Quaternion.identity, parent);

            return instantiatedPrefab;
        }
    }
}