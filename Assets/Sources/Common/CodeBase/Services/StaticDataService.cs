using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private Dictionary<HexagonStackTemplate, HexagonStackConfig> _stackStaticData;
        private Dictionary<HexagonTileType, HexagonTileConfig> _hexagonTileStaticData;
        
        private readonly IResourceLoader _resourceLoader;

        public GameConfig GameConfig { get; private set; }

        public StaticDataService(IResourceLoader resourceLoader) => 
            _resourceLoader = resourceLoader;

        public void Initialize()
        {
            GameConfig = _resourceLoader.LoadAsset<GameConfig>(StaticDataPaths.GameConfig);

            _stackStaticData = _resourceLoader.LoadAllAssets<HexagonStackConfig>(StaticDataPaths.StackConfig)
                .ToDictionary(x => x.Template, x => x);
            
            _hexagonTileStaticData = _resourceLoader.LoadAsset<HexagonTileStaticData>(StaticDataPaths.HexagonTileStaticData)
                .Configs
                .ToDictionary(x => x.TileType, x => x);
        }


        public HexagonStackConfig ForHexagonStack(HexagonStackTemplate gridTemplate) =>
            _stackStaticData.GetValueOrDefault(gridTemplate);
        
        public HexagonTileConfig ForHexagonStack(HexagonTileType tileType) =>
            _hexagonTileStaticData.GetValueOrDefault(tileType);
    }
}