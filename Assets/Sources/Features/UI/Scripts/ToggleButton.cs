using UnityEngine;
using UnityEngine.UI;

namespace Sources.Features.UI.Scripts
{
    public abstract class ToggleButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _switchedOnImage;
        [SerializeField] private Image _switchedOffImage;

        protected bool IsOn = true;

        private void Awake() => 
            _button.onClick.AddListener(Toggle);

        private void OnDestroy() =>
            CleanUp();

        private void CleanUp() => 
            _button.onClick.RemoveAllListeners();

        private void Toggle()
        {
            IsOn = !IsOn;

            UpdateVisualState();

            if (IsOn)
            {
                ToggleOn();            
            }
            else
            {
                ToggleOff();
            }
        }

        protected void UpdateVisualState()
        {
            if (IsOn)
            {
                _switchedOnImage.enabled = true;
                _switchedOffImage.enabled = false;
            }
            else
            {
                _switchedOnImage.enabled = false;
                _switchedOffImage.enabled = true;
            }
        }
    
        protected virtual void ToggleOn() { }
        protected virtual void ToggleOff() { }
    }
}