using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class GameFactory : IGameFactory
    {
        private readonly IInstantiator _instantiator;

        public GameFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public GameObject CreateHexagonStack(Vector3 position, Transform parent) => 
            Instantiate(Assets.HexagonPrefab, position, parent);

        public GameObject CreateHexagon(Vector3 position, Transform parent) => 
            Instantiate(Assets.HexagonPrefab, position, parent);

        private GameObject Instantiate(string assetPath, Vector3 at, Transform parent = null) => 
            _instantiator.InstantiatePrefabResource(assetPath, at, Quaternion.identity, parent);
    }
}