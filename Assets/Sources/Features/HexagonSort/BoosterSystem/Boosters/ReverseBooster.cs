using Sources.Features.HexagonSort.BoosterSystem.Activation;

namespace Sources.Features.HexagonSort.BoosterSystem.Boosters
{
    public class ReverseBooster : IBooster
    {
        private readonly BoosterContext _context;

        public BoosterType Type => BoosterType.ReverseBooster;

        public bool IsActive { get; private set; }

        public ReverseBooster(BoosterContext context)
        {
            _context = context;
        }
    
        public bool TryActivate()
        {
            if (CanActivate())
            {
                _context.StackSpawner.SpawnNewStacks();
            
                IsActive = true;
                return true;
            }
        
            return false;
        }

        public bool TryFinish()
        {
            if (IsActive)
            {
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
}