using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.HexagonStack.StackGenerator.Scripts;

namespace Sources.Common.CodeBase.Services
{
    public interface IStaticDataService
    {
        HexagonStackConfig ForHexagonStack(HexagonStackTemplate stackTemplate);
        GameConfig GameConfig { get; }
    }
}