using Sources.Features.HexagonSort.GridGenerator.Scripts;
using Sources.Features.HexagonSort.Scripts;
using Sources.Features.Level.Scripts;

namespace Sources.Common.CodeBase.Services
{
    public interface IStaticDataService
    {
        GridConfig ForGrid(GridTemplate gridTemplate);
        HexagonStackConfig ForHexagonStack(HexagonStackTemplate gridTemplate);
        LevelConfig ForLevel(string levelName);
    }
}