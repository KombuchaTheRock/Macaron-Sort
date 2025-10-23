using System;
using System.Collections.Generic;
using System.Linq;
using Sources.Common.CodeBase.Services.PlayerProgress.Data;
using Sources.Common.CodeBase.Services.StaticData;
using Sources.Features.HexagonSort.BoosterSystem.Activation;

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

        public PersistentProgressData CreatePersistentProgressData() => 
            new(GetNewPlayerData(), GetNewWorldData());

        public ControlPointProgressData CreateControlPointProgressData() => 
            new(GetNewPlayerData(), GetNewWorldData());

        private PlayerData GetNewPlayerData()
        {
            List<BoosterCount> boosterCounts = GetInitialBoosterCounts();
            return new PlayerData(_initialScore, _initialLevel, boosterCounts);
        }

        private static WorldData GetNewWorldData()
        {
            return new WorldData(
                new StacksData(
                    new List<PlacedStackData>(),
                    new List<FreeStackDataData>()),
                new GridData(new List<CellData>())
            );
        }

        private List<BoosterCount> GetInitialBoosterCounts()
        {
            List<BoosterCount> boosterCounts = new BoosterCount[]
            {
                new(BoosterType.ArrowBooster, 10),
                new(BoosterType.RocketBooster, 10),
                new(BoosterType.ReverseBooster, 10),
            }.ToList();
            
            return boosterCounts;
        }
    }
}