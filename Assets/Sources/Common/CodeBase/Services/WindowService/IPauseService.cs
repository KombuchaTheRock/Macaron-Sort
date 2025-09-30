using System;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public interface IPauseService
    {
        event Action Paused;
        event Action Unpaused;
        bool IsPaused { get; }
        void Pause();
        void Unpause();
    }
}