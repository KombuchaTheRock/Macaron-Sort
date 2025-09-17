using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackGenerator.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridGenerator.Scripts
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private MeshColor _meshColor;
        
        private HexagonStack _stack;
        private Color _normal;
        private Color _highlight;

        public bool IsOccupied => _stack != null;

        private void Start() => 
            DisableHighlight();

        public void InitializeColors(Color normal, Color highlight)
        {
            _highlight = highlight;
            _normal = normal;
        }
        
        public void SetStack(HexagonStack stack) =>
            _stack = stack;

        public void EnableHighlight() =>
            _meshColor.Set(_highlight);

        public void DisableHighlight() =>
            _meshColor.Set(_normal);
    }
}