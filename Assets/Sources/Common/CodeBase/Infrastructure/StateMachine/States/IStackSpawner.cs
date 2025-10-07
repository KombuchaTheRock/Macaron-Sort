namespace Sources.Common.CodeBase.Infrastructure.StateMachine.States
{
    public interface IStackSpawner
    {
        void SpawnNewStacks();
        void StopSpawn();
    }
}