using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Common.CodeBase.Services.StaticData;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class SaveDataFactory : ISaveDataFactory
    {
        private readonly IStaticDataService _staticData;

        public SaveDataFactory(IStaticDataService staticData)
        {
            _staticData = staticData;
        }

        public PersistentProgressData CreatePersistentProgressData()
        {
            return new PersistentProgressData(new PlayerData(0, 1), new WorldData(
                new StacksData(
                    new List<PlacedStackData>(),
                    new List<FreeStackDataData>())
                )
            );
        }

        public ControlPointProgressData CreateControlPointProgressData()
        {
            return new ControlPointProgressData(new PlayerData(0, 1), new WorldData(
                    new StacksData(
                        new List<PlacedStackData>(),
                        new List<FreeStackDataData>())
                )
            );
        }
    }
}