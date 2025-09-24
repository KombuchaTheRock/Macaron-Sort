namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public interface ISaveDataFactory
    {
        PlayerData CreateNewPlayerData();
        WorldData CreateNewWorldData();
    }
}