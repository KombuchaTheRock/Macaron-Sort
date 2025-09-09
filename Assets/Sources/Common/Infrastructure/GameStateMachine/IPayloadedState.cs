namespace Sources.Common.Infrastructure
{
    public interface IPayloadedState<TPayload> :IExitableState
    {
        void Enter(TPayload isFreshRestartNeeded);
    }
}