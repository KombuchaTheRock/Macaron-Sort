using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int _score;
        [SerializeField] private int _level;
        [SerializeField] private List<BoosterCount> _boosters;

        public IReadOnlyList<BoosterCount> Boosters => _boosters;
        public int Score => _score;
        public int Level => _level;

        public PlayerData(int score, int level, List<BoosterCount> boosters)
        {
            _score = score;
            _level = level;
            _boosters = boosters;
        }

        public void UpdateLevelData(PlayerLevel playerLevel)
        {
            _level = playerLevel.Level;
            _score = playerLevel.Score;
        }

        public void UpdateBoosterData(List<BoosterCount> boosters) => 
            _boosters = boosters;
    }
}