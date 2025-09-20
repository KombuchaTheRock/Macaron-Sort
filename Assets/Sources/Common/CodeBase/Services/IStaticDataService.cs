using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Common.CodeBase.Services
{
    public interface IStaticDataService
    {
        HexagonStackConfig ForHexagonStack(HexagonStackTemplate stackTemplate);
        GameConfig GameConfig { get; }
        HexagonTileConfig ForHexagonTle(HexagonTileType tileType);
    }
}