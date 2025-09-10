using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    public class Hexagon : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Color Color => _meshRenderer.material.color;

        public void SetColor(Color color) => 
            _meshRenderer.material.color = color;
    }
}
