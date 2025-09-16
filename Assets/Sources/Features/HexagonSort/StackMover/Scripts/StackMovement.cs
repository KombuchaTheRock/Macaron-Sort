using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
{
    public class StackMovement : MonoBehaviour
    {
        private float _movementSpeed = 30f;
        private TweenerCore<Vector3, Vector3, VectorOptions> _transitionToInitialAnim;

        public bool CanMove { get; set; }

        private void Awake() =>
            CanMove = true;

        public void FollowingTarget(Vector3 target)
        {
            if (CanMove == false)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, target, _movementSpeed * Time.deltaTime);
        }

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null)
        {
            if (CanMove == false)
                return;

            float transitionDuration = (transform.position - targetPosition).magnitude / speed;

            _transitionToInitialAnim?.Complete();

            _transitionToInitialAnim = transform
                .DOMove(targetPosition, transitionDuration)
                .SetEase(Ease.OutBounce)
                .Play()
                .OnComplete(() => onComplete?.Invoke())
                .SetLink(gameObject);
        }
    }
}