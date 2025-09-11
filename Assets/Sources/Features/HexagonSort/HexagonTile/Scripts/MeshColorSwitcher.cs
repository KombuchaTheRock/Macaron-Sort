using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonTile.Scripts
{
    public class MeshColorSwitcher : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        private void Awake()
        {
            if (_meshRenderer == null)
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        public Color Color => _meshRenderer.material.color;
    
        public void SetColor(Color color) =>
            _meshRenderer.material.color = color;
    }
}
