using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

namespace Sources.Features.HexagonSort.StackCompleter
{
    public class HexagonDeleteAnimation
    {
        public TweenerCore<Vector3, Vector3, VectorOptions> DeleteAnimation(Hexagon hexagon, float delay,
            float duration, Action onCompleted = null)
        {
            return hexagon.transform.DOScale(0f, duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay)
                .OnComplete(() => onCompleted?.Invoke())
                .SetLink(hexagon.gameObject);
        }
    }
}