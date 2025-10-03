using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackCompletion
    {
        public event Action<int> StackCompleted;
        public event Action DeleteAnimationCompleted;

        private readonly MergeAnimation _mergeAnimation;
        private int _hexagonsCountForComplete;

        public StackCompletion(int hexagonsCountForComplete)
        {
            _hexagonsCountForComplete = hexagonsCountForComplete;
            _mergeAnimation = new MergeAnimation();
        }

        public IEnumerator CheckStackForCompleteRoutine(MergeCandidate mergeCandidate)
        {
            if (mergeCandidate.Stack.Hexagons.Count < _hexagonsCountForComplete)
                yield break;

            Tween deleteAnimation = null;
            HexagonTileType topHexagonTileType = mergeCandidate.Stack.TopHexagon.TileType;
            
            List<Hexagon> similarHexagons = StackAnalyze.GetSimilarHexagons(mergeCandidate.Stack,
                topHexagonTileType);

            bool isMonoType =
                StackAnalyze.CheckForMonoType(mergeCandidate.Stack, topHexagonTileType);
            
            if (similarHexagons.Count < _hexagonsCountForComplete)
                yield break;

            float delay = 0;

            foreach (Hexagon hexagon in similarHexagons)
            {
                deleteAnimation =
                    _mergeAnimation.HexagonDeleteAnimation(hexagon, delay, 0.2f, DeleteAnimationCompleted);

                deleteAnimation.Play();
                delay += 0.03f;
            }

            yield return deleteAnimation.WaitForCompletion();

            int score = StackAnalyze.CalculateScore(similarHexagons);
            StackCompletePopUp(mergeCandidate);

            DeleteHexagons(mergeCandidate.Stack, similarHexagons);
            
            if (isMonoType)
                DeleteStack(mergeCandidate);

            StackCompleted?.Invoke(score);
        }

        private void DeleteStack(MergeCandidate mergeCandidate)
        {
            mergeCandidate.Cell.SetStack(null);

            if (mergeCandidate.Stack is not null)
                Object.Destroy(mergeCandidate.Stack.gameObject);
        }
        
        private void DeleteHexagons(HexagonStack stack, List<Hexagon> similarHexagons)
        {
            foreach (Hexagon hexagon in similarHexagons.Where(stack.Contains))
            {
                stack.Remove(hexagon);
                Object.Destroy(hexagon.gameObject);
            }
        }
        
        private static void StackCompletePopUp(MergeCandidate mergeCandidate)
        {
            Vector3 popUpPosition = mergeCandidate.Stack.TopHexagon.transform.position;

            GameObject popUp = Resources.Load<GameObject>("StackGenerator/Prefab/StackCountPopUp");

            TextMeshPro popUpText = popUp.GetComponentInChildren<TextMeshPro>();
            popUpText.text = mergeCandidate.Stack.Hexagons.Count.ToString();

            Object.Instantiate(popUp, popUpPosition, popUp.transform.rotation, mergeCandidate.Cell.transform);
        }
    }
}