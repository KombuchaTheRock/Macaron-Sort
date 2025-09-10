using System.Collections.Generic;
using System.Linq;
using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class StaticDataService : IStaticDataService, IInitializable
    {
        private Dictionary<GridTemplate, GridConfig> _gridStaticData;
        private readonly IResourceLoader _resourceLoader;

        public StaticDataService(IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public void Initialize()
        {
            _gridStaticData = _resourceLoader.LoadAllAssets<GridConfig>(AssetsPaths.GridConfig)
                .ToDictionary(x => x.Template, x => x);
        }

        public GridConfig ForGrid(GridTemplate gridTemplate) =>
            _gridStaticData.GetValueOrDefault(gridTemplate);
    }
}