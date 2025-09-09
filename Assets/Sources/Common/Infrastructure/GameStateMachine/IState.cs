namespace Sources.Common.Infrastructure
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}