public interface IBooster
{
    BoosterType Type { get; }
    bool IsActive { get; }
    bool TryActivate();
    bool TryDeactivate();
}