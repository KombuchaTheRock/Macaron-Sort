using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public class GridCell : MonoBehaviour
    {
        [SerializeField] private MeshColor _meshColor;

        private Color[] _normal;
        private Color _highlight;

        public Vector2Int PositionOnGrid { get; private set; }
        public HexagonStack Stack { get; private set; }

        public bool IsOccupied => Stack != null;

        private void Start() => 
            DisableHighlight();

        public void InitializeColors(Color normal, Color highlight)
        {
            _highlight = highlight;
            _normal = _meshColor.Colors;
        }

        public void InitializeGridPosition(Vector2Int gridPosition) =>
        PositionOnGrid = gridPosition;
        
        public void SetStack(HexagonStack stack) =>
            Stack = stack;

        public void RemoveStack() =>
            Stack = null;

        public void EnableHighlight()
        {
            _meshColor.SetMaterialsColor(_highlight);
            _meshColor.Set(_highlight);
        }

        public void DisableHighlight() =>
            _meshColor.SetMaterialsColor(_normal);
    }
}