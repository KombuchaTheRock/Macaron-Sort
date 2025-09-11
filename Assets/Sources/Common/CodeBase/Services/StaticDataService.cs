using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Paths;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Scripts;
using Sources.Features.Level.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private Dictionary<GridTemplate, GridConfig> _gridStaticData;
        private Dictionary<HexagonStackTemplate, HexagonStackConfig> _stackStaticData;
        private Dictionary<string, LevelConfig> _levelStaticData;
        
        private readonly IResourceLoader _resourceLoader;

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public void Initialize()
        {
            _gridStaticData = _resourceLoader.LoadAllAssets<GridConfig>(StaticDataPaths.GridConfig)
                .ToDictionary(x => x.Template, x => x);
            
            _stackStaticData = _resourceLoader.LoadAllAssets<HexagonStackConfig>(StaticDataPaths.StackConfig)
                .ToDictionary(x => x.Template, x => x);
            
            _levelStaticData = _resourceLoader.LoadAllAssets<LevelConfig>(StaticDataPaths.LevelConfig)
                .ToDictionary(x => x.LevelName, x => x);
        }

        public GridConfig ForGrid(GridTemplate gridTemplate) =>
            _gridStaticData.GetValueOrDefault(gridTemplate);
        
        public HexagonStackConfig ForHexagonStack(HexagonStackTemplate gridTemplate) =>
            _stackStaticData.GetValueOrDefault(gridTemplate);
    
        public LevelConfig ForLevel(string levelName) =>
            _levelStaticData.GetValueOrDefault(levelName);
    }
}