using UnityEngine;

namespace Sources.Features.HexagonSort.Scripts
{
    public class Hexagon : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Color Color
        {
            get => _meshRenderer.material.color;
            set => _meshRenderer.material.color = value;
        }
    }
}
