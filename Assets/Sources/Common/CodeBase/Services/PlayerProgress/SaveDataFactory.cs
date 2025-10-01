using System.Collections.Generic;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Common.CodeBase.Services.StaticData;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    public class SaveDataFactory : ISaveDataFactory
    {
        private readonly int _initialLevel;
        private readonly int _initialScore;
        
        public SaveDataFactory(IStaticDataService staticData)
        {
            _initialLevel = staticData.GameConfig.PlayerDataConfig.InitialLevel;
            _initialScore = staticData.GameConfig.PlayerDataConfig.InitialScore;
        }

        public PersistentProgressData CreatePersistentProgressData()
        {
            return new PersistentProgressData(new PlayerData(_initialScore, _initialLevel), new WorldData(
                new StacksData(
                    new List<PlacedStackData>(),
                    new List<FreeStackDataData>())
                )
            );
        }

        public ControlPointProgressData CreateControlPointProgressData()
        {
            return new ControlPointProgressData(new PlayerData(_initialScore, _initialLevel), new WorldData(
                    new StacksData(
                        new List<PlacedStackData>(),
                        new List<FreeStackDataData>())
                )
            );
        }
    }
}