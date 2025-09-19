using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.Scripts
{
    public class HexagonStackCollider : MonoBehaviour
    {
        [SerializeField] private MeshCollider _meshCollider;

        private Vector3 _originalScale;
        private Vector3 _originalPosition;
        private float _originalHeight;
        private float _originalPivotOffset;

        public float OriginalHeight => _originalHeight;

        private void Awake()
        {
            _originalScale = _meshCollider.transform.localScale;
            _originalHeight = _meshCollider.sharedMesh.bounds.size.y;
        
            _originalPosition = _meshCollider.transform.localPosition;
            Bounds meshBounds = _meshCollider.sharedMesh.bounds;
            _originalPivotOffset = meshBounds.center.y - meshBounds.min.y;
        }

        public void SetHeight(float heightMultiplier)
        {
            Vector3 newScale = _originalScale;
            newScale.y = _originalScale.y * heightMultiplier;

            _meshCollider.transform.localScale = newScale;
        
            float heightDifference = (newScale.y - _originalScale.y) * _originalPivotOffset;
            Vector3 newPosition = _originalPosition;
            newPosition.y += heightDifference;
            _meshCollider.transform.localPosition = newPosition;
        }
    }
}