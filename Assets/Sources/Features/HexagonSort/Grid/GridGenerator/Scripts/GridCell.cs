using Sources.Features.HexagonSort.HexagonStack.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Grid.GridGenerator.Scripts
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private MeshColor _meshColor;

        private HexagonStack.StackGenerator.Scripts.HexagonStack _stack;
        private Color[] _normal;
        private Color _highlight;

        public bool IsOccupied => _stack != null;

        private void Start()
        {
            DisableHighlight();
        }

        public void InitializeColors(Color normal, Color highlight)
        {
            _highlight = highlight;
            _normal = _meshColor.Colors;
        }

        public void SetStack(HexagonStack.StackGenerator.Scripts.HexagonStack stack) =>
            _stack = stack;

        public void RemoveStack() =>
            _stack = null;

        public void EnableHighlight()
        {
            _meshColor.SetMaterialsColor(_highlight);
            _meshColor.Set(_highlight);
        }

        public void DisableHighlight() =>
            _meshColor.SetMaterialsColor(_normal);
    }
}