using System;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services
{
    public class StandaloneInput : IInputService, ITickable
    {
        public event Action CursorDown;
        public event Action CursorUp;
        
        public bool IsCursorHold => Input.GetMouseButton(0);
        public Vector2 CursorPosition => Input.mousePosition;
        
        public void Tick() => 
            HandleInput();

        private void HandleInput()
        {
            if (Input.GetMouseButtonDown(0))
                CursorDown?.Invoke();
            else if (Input.GetMouseButtonUp(0))
                CursorUp?.Invoke();
        }
    }
}