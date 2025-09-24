namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class SaveDataFactory : ISaveDataFactory
    {
        private readonly IStaticDataService _staticData;

        public SaveDataFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public PlayerData CreateNewPlayerData() =>
            new();

        public WorldData CreateNewWorldData() =>
            new();
    }
}