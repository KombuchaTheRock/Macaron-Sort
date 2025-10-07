using UnityEngine;
using UnityEngine.UI;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button[] CloseButtons;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            foreach (Button closeButton in CloseButtons) 
                closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        protected virtual void OnCloseButtonClicked() =>
            Destroy(gameObject);
    }
}