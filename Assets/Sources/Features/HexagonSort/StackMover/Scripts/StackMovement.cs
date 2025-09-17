using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackMover.Scripts
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

            void OnCompleteAction()
            {
                CanMove = true;
                onComplete?.Invoke();
            }
            
            CanMove = false;
            _transitionToInitialAnim = transform
                .DOMove(targetPosition, transitionDuration)
                .SetEase(Ease.OutQuad)
                .Play()
                .OnComplete(OnCompleteAction)
                .SetLink(gameObject);
        }
    }
}