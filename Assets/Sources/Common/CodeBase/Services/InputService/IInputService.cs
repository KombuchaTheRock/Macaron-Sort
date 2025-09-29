using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.InputService
{
    public interface IInputService
    {
        event Action CursorDown;
        event Action CursorUp;
        bool IsCursorHold { get; }
        Vector2 CursorPosition { get; }
    }
}