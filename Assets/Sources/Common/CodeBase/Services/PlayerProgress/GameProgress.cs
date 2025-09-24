namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class GameProgress
    {
        public PlayerData PlayerData { get; }
        public WorldData WorldData { get; }

        public GameProgress(PlayerData playerData, WorldData worldData)
        {
            PlayerData = playerData;
            WorldData = worldData;
        }
    }
}