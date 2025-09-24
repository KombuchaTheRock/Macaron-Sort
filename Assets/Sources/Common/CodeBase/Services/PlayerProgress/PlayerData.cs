using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress
{
    [Serializable]
    public class PlayerData
    {
        [SerializeField] private int _score;
        [SerializeField] private int _level = 1;

        public int Score => _score;
        public int Level => _level;

        public void UpdateData(PlayerLevel playerLevel)
        {
            _level = playerLevel.Level;
            _score = playerLevel.Score;
        }
    }
}