using System;
using System.Collections.Generic;
using Sources.Features.HexagonSort.HexagonStackSystem.StackMover.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStackSystem.Scripts
{
    public class HexagonStack : MonoBehaviour
    {
        public event Action SizeChanged;
        
        [SerializeField] private StackMovement _movement;
        [SerializeField] private HexagonStackCollider _collider;
        [SerializeField] private StackSizeViewer _stackSizeViewer;
        
        public IReadOnlyList<Hexagon> Hexagons => _hexagons;
        public float OffsetBetweenTiles { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        
        public Hexagon TopHexagon => Hexagons[^1];
        public bool CanMove => _movement.CanMove;

        private List<Hexagon> _hexagons = new();

        private void Awake()
        {
            _stackSizeViewer.Initialize(this);
            _movement.StartAnimation(transform.position);
        }

        public void ShowDisplayedSize() => 
            _stackSizeViewer.Show();
        
        public void HideDisplayedSize() => 
            _stackSizeViewer.Hide();

        public void SetInitialPosition(Vector3 position) =>
            InitialPosition = position;

        public void SetOffsetBetweenTiles(float offset) =>
        OffsetBetweenTiles = offset;
        
        public void DisableMovement() =>
            _movement.DisableMovement();

        public void FollowingTarget(Vector3 target, float speed) =>
            _movement.FollowingTarget(target, speed);

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null) =>
            _movement.MoveToTarget(targetPosition, speed, onComplete);

        public void Add(Hexagon hexagon)
        {
            hexagon.transform.rotation = transform.rotation;
            _hexagons.Add(hexagon);
            ChangeColliderSize(hexagon.Height);
            
            SizeChanged?.Invoke();
        }

        public bool Contains(Hexagon hexagon) =>
            _hexagons.Contains(hexagon);

        public void Remove(Hexagon hexagon)
        {
            _hexagons.Remove(hexagon);
            ChangeColliderSize(hexagon.Height);
            
            SizeChanged?.Invoke();
            
            if (_hexagons.Count <= 0) 
                Destroy(gameObject);
        }

        private void ChangeColliderSize(float hexagonHeight)
        {
            float stackHeight = _hexagons.Count * (hexagonHeight + (OffsetBetweenTiles - hexagonHeight));
            float stackColliderHeightMultiplier = stackHeight / _collider.OriginalHeight;

            _collider.SetHeight(stackColliderHeightMultiplier);
        }
    }
}