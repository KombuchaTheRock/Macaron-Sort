using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.HexagonTile.Scripts
{
    public class MeshColor : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;

        public Color Color => _meshRenderer.material.color;

        public Color[] Colors
        {
            get
            {
                if (_meshRenderer == null || _meshRenderer.materials.Length == 0)
                    return new Color[0];

                Color[] colors = new Color[_meshRenderer.materials.Length];
                for (int i = 0; i < _meshRenderer.materials.Length; i++)
                {
                    colors[i] = _meshRenderer.materials[i].color;
                }
                return colors;
            }
        }
        
        public void Set(Color color) =>
            _meshRenderer.material.color = color;

        public void SetMaterialsColor(Color[] colors)
        {
            for (int i = 0; i < _meshRenderer.materials.Length; i++) 
                _meshRenderer.materials[i].color = colors[i];
        }
        
        public void SetMaterialsColor(Color color)
        {
            foreach (Material material in _meshRenderer.materials)
                material.color = color;
        }
    }
}
