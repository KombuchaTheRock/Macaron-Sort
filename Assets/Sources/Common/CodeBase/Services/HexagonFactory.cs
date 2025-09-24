using System.Collections.Generic;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class HexagonFactory : BaseFactory, IHexagonFactory
    {
        private readonly IStaticDataService _staticData;
        private const string StacksRootName = "Stacks";
        public List<HexagonStack> Stacks { get; private set; }

        public Transform CreateStacksRoot() => 
            new GameObject(StacksRootName).transform;

        public HexagonFactory(IInstantiator instantiator, IResourceLoader resourceLoader, IStaticDataService staticData) : base(instantiator, resourceLoader)
        {
            _staticData = staticData;
            Stacks = new List<HexagonStack>();
        }

        public Hexagon CreateHexagon(Vector3 position, HexagonTileType tileType, Transform parent)
        {
            HexagonTileData hexagonTileData = _staticData.ForHexagonTle(tileType);
            Hexagon hexagonPrefab = hexagonTileData.HexagonPrefab;
            Hexagon hexagon = Instantiate<Hexagon>(hexagonPrefab.gameObject, position, parent);

            hexagon.Initialize(tileType, hexagonTileData.ScoreAmount);

            return hexagon;
        }
        
        public HexagonStack CreateHexagonStack(Vector3 position, Transform parent, float offsetBetweenTiles)
        {
            HexagonStack hexagonStack = Instantiate<HexagonStack>(AssetsPaths.StackPrefab, position, parent);
            hexagonStack.SetInitialPosition(position);
            hexagonStack.SetOffsetBetweenTiles(offsetBetweenTiles);

            Stacks.Add(hexagonStack);
            return hexagonStack;
        }
    }
}