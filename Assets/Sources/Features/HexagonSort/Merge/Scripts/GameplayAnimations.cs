using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class GameplayAnimations
    {
        public Tween HexagonMergeAnimation(HexagonStack stack, float targetY,
            Hexagon hexagon, float duration, float delay, Action onCompleted = null)
        {
            Quaternion hexagonInitialRotation = hexagon.transform.rotation;

            (Quaternion targetRotation, Vector3 targetPosition) =
                CalculateAnimationTargetTransform(stack.transform, hexagon.transform, targetY);

            Sequence sequence = DOTween.Sequence();

            sequence.PrependInterval(delay);

            sequence.Append(hexagon.transform.DORotateQuaternion(targetRotation, duration)
                .SetEase(Ease.InOutSine));

            sequence.Join(hexagon.transform.DOJump(targetPosition, 0.75f, 1, duration)
                .SetEase(Ease.InOutSine));

            sequence.SetLink(hexagon.gameObject)
                .OnComplete(() =>
                {
                    onCompleted?.Invoke();
                    if (hexagon != null) hexagon.transform.rotation = hexagonInitialRotation;
                });

            return sequence;
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> HexagonDeleteAnimation(Hexagon hexagon, float delay,
            float duration, Action onCompleted = null)
        {
            return hexagon.transform.DOScale(0f, duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay)
                .OnComplete(() => onCompleted?.Invoke())
                .SetLink(hexagon.gameObject);
        }

        private (Quaternion targetRotation, Vector3 targetPosition) CalculateAnimationTargetTransform
            (Transform stack, Transform hexagon, float targetY)
        {
            Vector3 targetPosition = new(stack.position.x, targetY, stack.position.z);
            Vector3 movementDirection = targetPosition - hexagon.position;

            Vector3 horizontalDir = new Vector3(movementDirection.x, 0f, movementDirection.z).normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, horizontalDir);

            Quaternion currentRotation = hexagon.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(-180, rotationAxis) * currentRotation;

            return (targetRotation, targetPosition);
        }
    }
}