using Sources.Features.HexagonSort.BoosterSystem.Activation;

namespace Sources.Features.HexagonSort.BoosterSystem.Boosters
{
    public interface IBooster
    {
        BoosterType Type { get; }
        bool IsActive { get; }
        bool TryActivate();
        bool TryFinish();
    }
}