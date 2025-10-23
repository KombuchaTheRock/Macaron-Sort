using System;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Color = UnityEngine.Color;

namespace Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts
{
    public class GridCell : MonoBehaviour
    {
        public event Action StackRemoved;
        
        [SerializeField] private MeshColor _meshColor;
        [SerializeField] private GridCellLockView _gridCellLockView;
        
        private Color[] _normal;
        private Color _highlight;

        public Vector2Int PositionOnGrid { get; private set; }
        public HexagonStack Stack { get; private set; }

        public bool IsOccupied => Stack != null || _isBlocked;
        
        private bool _isBlocked;
                                  
        private void Start() => 
            DisableHighlight();

        public void InitializeColors(Color normal, Color highlight)
        {
            _highlight = highlight;
            _normal = _meshColor.Colors;
        }

        public void InitializeGridPosition(Vector2Int gridPosition) =>
        PositionOnGrid = gridPosition;

        public void Block()
        {
            _isBlocked = true;
            _gridCellLockView.Show();
        }

        public void Unblock()
        {
            _isBlocked = false;
            _gridCellLockView.Hide();
        }
        
        public void OccupyCell(HexagonStack stack) => 
            Stack = stack;

        public void FreeCell()
        {
            Stack = null;
            StackRemoved?.Invoke();
        }

        public void EnableHighlight()
        {
            _meshColor.SetMaterialsColor(_highlight);
            _meshColor.Set(_highlight);
        }

        public void DisableHighlight() =>
            _meshColor.SetMaterialsColor(_normal);
    }
}