namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}