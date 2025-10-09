public class RocketBooster : IBooster
{
    private readonly BoosterContext _context;
    
    public BoosterType Type => BoosterType.RocketBooster;
    public bool IsActive { get; private set; }

    public RocketBooster(BoosterContext context) => 
        _context = context;

    public bool TryActivate()
    {
        if (CanActivate() == false)
            return false;
        
        _context.GridRotator.enabled = false;
        _context.StackMover.Deactivate();
        _context.StackCompleter.Activate();
        
        IsActive = true;
        
        return true;
    }

    public bool TryDeactivate()
    {
        if (IsActive)
        {
            _context.GridRotator.enabled = true;
            _context.StackMover.Activate();
            _context.StackCompleter.Deactivate();

            IsActive = false;
            _context.BoosterCounter.RemoveBooster(Type);

            return true;
        }

        return false;
    }

    private bool CanActivate()
    {
        return _context.BoosterCounter.BoostersCount[Type] > 0
               && IsActive == false;
    }
}