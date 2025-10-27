using Sources.Common.CodeBase.Infrastructure;
using Sources.Common.CodeBase.Services.SoundService;
using Sources.Common.CodeBase.Services.WindowService;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Common.CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        HexagonStackConfig ForHexagonStack(HexagonStackTemplate stackTemplate);
        GameConfig GameConfig { get; }
        SoundsStaticData SoundsData { get; }
        HexagonTileData ForHexagonTile(HexagonTileType tileType);
        WindowConfig ForWindow(WindowID windowID);
    }
}