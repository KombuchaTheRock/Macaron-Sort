namespace Sources.Common.CodeBase.Infrastructure.StateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}