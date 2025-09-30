using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class PauseService : IPauseService
    {
        public event Action Paused;
        public event Action Unpaused;
        
        public bool IsPaused { get; private set; }
        
        public void Pause()
        {
            Time.timeScale = 0f;
            IsPaused = true;
            Paused?.Invoke();
        }

        public void Unpause()
        {
            Time.timeScale = 1f;
            IsPaused = false;
            Unpaused?.Invoke();
        }
    }
}