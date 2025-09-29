using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Button ToControlPointButton => _toControlPointButton;
    [SerializeField] private Button _toControlPointButton;
}
