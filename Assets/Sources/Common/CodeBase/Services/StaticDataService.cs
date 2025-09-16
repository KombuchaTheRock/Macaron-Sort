using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private Dictionary<HexagonStackTemplate, HexagonStackConfig> _stackStaticData;
        
        private readonly IResourceLoader _resourceLoader;
        
        public GameConfig GameConfig { get; private set; }

        public StaticDataService(IResourceLoader resourceLoader) => 
            _resourceLoader = resourceLoader;

        public void Initialize()
        {
            GameConfig = _resourceLoader.LoadAsset<GameConfig>(StaticDataPaths.GameConfig);

            _stackStaticData = _resourceLoader.LoadAllAssets<HexagonStackConfig>(StaticDataPaths.StackConfig)
                .ToDictionary(x => x.Template, x => x);
        }


        public HexagonStackConfig ForHexagonStack(HexagonStackTemplate gridTemplate) =>
            _stackStaticData.GetValueOrDefault(gridTemplate);
    }
}