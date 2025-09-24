using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class PlayerData : ISaveData
    {
        [SerializeField] private int _score;
        [SerializeField] private int _level;

        public int Score => _score;
        public int Level => _level;

        public void UpdateData(PlayerLevel playerLevel)
        {
            _level = playerLevel.Level;
            _score = playerLevel.Score;
        }
    }
}