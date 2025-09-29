using System.Collections.Generic;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.WindowService
{
    [CreateAssetMenu(fileName = "WindowsStaticData", menuName = "StaticData/WindowsStaticData")]
    public class WindowStaticData : ScriptableObject
    {
        [field: SerializeField] public List<WindowConfig> Configs { get; private set; }
    }
}