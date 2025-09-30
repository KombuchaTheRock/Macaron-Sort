using UnityEngine;
using UnityEngine.UI;

namespace Sources.Common.CodeBase.Services.WindowService
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField] protected Button CloseButton;

        private void Awake()
        {
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            CloseButton.onClick.AddListener(OnClicked);
        }

        protected virtual void OnClicked() =>
            Destroy(gameObject);
    }
}