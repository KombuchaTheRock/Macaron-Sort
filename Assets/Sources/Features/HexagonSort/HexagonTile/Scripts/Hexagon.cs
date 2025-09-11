using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    public class Hexagon : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        
        public HexagonStack Stack { get; private set; }
        
        public void SetStack(HexagonStack stack) => 
            Stack = stack;
    }
}
