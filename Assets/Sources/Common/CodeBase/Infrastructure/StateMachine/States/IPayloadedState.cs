namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public interface IPayloadedState<TPayload> :IExitableState
    {
        void Enter(TPayload payload);
    }
}