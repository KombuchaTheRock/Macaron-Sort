using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Sources.Features.HexagonSort.HexagonStack.StackMover.Scripts
{
    public class StackMovement : MonoBehaviour
    {
        private TweenerCore<Vector3, Vector3, VectorOptions> _transitionToInitialAnim;

        public bool CanMove { get; private set; } = true;

        public void EnableMovement() =>
            CanMove = true;

        public void DisableMovement() =>
            CanMove = false;

        public void FollowingTarget(Vector3 target, float speed)
        {
            if (CanMove == false)
                return;

            transform.position =
                Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }

        public void MoveToTarget(Vector3 targetPosition, float speed, Action onComplete = null)
        {
            if (CanMove == false)
                return;

            float transitionDuration = (transform.position - targetPosition).magnitude / speed;
            _transitionToInitialAnim?.Complete();

            DisableMovement();
            _transitionToInitialAnim = TransitionToInitialAnim(targetPosition, onComplete, transitionDuration);
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> TransitionToInitialAnim(Vector3 targetPosition,
            Action onComplete, float transitionDuration)
        {
            return transform
                .DOMove(targetPosition, transitionDuration)
                .SetEase(Ease.OutQuad)
                .Play()
                .OnComplete(() => FinishMoveToTarget(onComplete))
                .SetLink(gameObject);
        }

        private void FinishMoveToTarget(Action onComplete)
        {
            EnableMovement();
            onComplete?.Invoke();
        }
    }
}