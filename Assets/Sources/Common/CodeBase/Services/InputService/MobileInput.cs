using System;
using UnityEngine;
using Zenject;

namespace Sources.Common.CodeBase.Services.InputService
{
    public class MobileInput : IInputService, ITickable
    {
        public event Action CursorDown;
        public event Action CursorUp;
        
        public bool IsCursorHold => CheckCursorHold();
        public Vector2 CursorPosition { get; private set; }

        public void Tick() => 
            HandleInput();

        private void HandleInput()
        {
            if (Input.touchCount <= 0)
                return;
    
            Touch touch = Input.GetTouch(0);
            CursorPosition = touch.position;

            SwitchTouchTo(touch.phase);
        }

        private void SwitchTouchTo(TouchPhase phase)
        {
            switch (phase)
            {
                case TouchPhase.Began:
                    CursorDown?.Invoke();
                    break;
            
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    CursorUp?.Invoke();
                    break;
            }
        }

        private bool CheckCursorHold()
        {
            return Input.touchCount > 0 && 
                   Input.GetTouch(0).phase != TouchPhase.Ended &&
                   Input.GetTouch(0).phase != TouchPhase.Canceled;
        }
    }
}