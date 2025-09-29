using UnityEngine;
using UnityEngine.UI;

namespace Sources.Features.UI.Scripts
{
    public class GameOverScreen : MonoBehaviour
    {
        public Button ToControlPointButton => _toControlPointButton;
        [SerializeField] private Button _toControlPointButton;
    }
}
