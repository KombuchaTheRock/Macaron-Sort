using Sources.Features.HexagonSort.GridGenerator.Scripts;

namespace Sources.Common.CodeBase.Services
{
    public interface IStaticDataService
    {
        GridConfig ForGrid(GridTemplate gridTemplate);
    }
}