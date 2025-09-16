using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    public class MeshColor : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Color Color => _meshRenderer.material.color;

        public void Set(Color color) =>
            _meshRenderer.material.color = color;
    }
}
