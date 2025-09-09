namespace Sources.Common.CodeBase.Infrastructure.StateMachine
{
    public interface IPayloadedState<TPayload> :IExitableState
    {
        void Enter(TPayload isFreshRestartNeeded);
    }
}