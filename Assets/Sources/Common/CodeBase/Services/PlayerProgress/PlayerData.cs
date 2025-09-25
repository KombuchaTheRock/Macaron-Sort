using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int _score;
        [SerializeField] private int _level;

        public int Score => _score;
        public int Level => _level;

        public PlayerData()
        {
            _score = 0;
            _level = 1;
        }
        
        public PlayerData(int score, int level)
        {
            _score = score;
            _level = level;
        }
        
        public void UpdateData(PlayerLevel playerLevel)
        {
            _level = playerLevel.Level;
            _score = playerLevel.Score;
        }
    }
}