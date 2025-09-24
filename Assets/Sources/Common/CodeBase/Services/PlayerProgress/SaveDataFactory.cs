namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class SaveDataFactory : ISaveDataFactory
    {
        private readonly IStaticDataService _staticData;

        public SaveDataFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public PersistentProgressData CreatePersistentProgressData() =>
            new(new PlayerData(), new WorldData());

        public ControlPointProgressData CreateControlPointProgressData() => 
            new(new PlayerData(), new WorldData());
    }
}