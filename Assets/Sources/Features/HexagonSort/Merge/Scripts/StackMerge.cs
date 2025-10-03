using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sources.Features.HexagonSort.HexagonTile.Scripts;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackMerge
    {
        public event Action MergeAnimationCompleted;
        
        private readonly MergeAnimation _mergeAnimation;

        public StackMerge(MergeAnimation mergeAnimation) =>
            _mergeAnimation = mergeAnimation;

        public IEnumerator MergeRoutine(MergeCandidate mergeCandidate, List<Hexagon> hexagonForMerge)
        {
            float offsetBetweenTiles = mergeCandidate.Stack.OffsetBetweenTiles;
            float initialY = mergeCandidate.Stack.Hexagons[^1].transform.position.y + offsetBetweenTiles;

            Sequence overallSequence = DOTween.Sequence();

            float delay = 0.2f;

            for (int i = 0; i < hexagonForMerge.Count; i++)
            {
                Hexagon hexagon = hexagonForMerge[i];

                float targetY = initialY + i * offsetBetweenTiles;

                float duration = 0.4f;
                
                Tween mergeTween = _mergeAnimation.HexagonMergeAnimation(mergeCandidate.Stack, 
                    targetY, 
                    hexagon,
                    duration, 
                    delay,
                    MergeAnimationCompleted);

                delay += 0.04f;

                overallSequence.Join(mergeTween);
            }

            overallSequence.Play();
            yield return overallSequence.WaitForCompletion();
        }
    }
}