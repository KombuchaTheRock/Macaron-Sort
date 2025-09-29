using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Common.CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        HexagonStackConfig ForHexagonStack(HexagonStackTemplate stackTemplate);
        GameConfig GameConfig { get; }
        HexagonTileData ForHexagonTle(HexagonTileType tileType);
    }
}