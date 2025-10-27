using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.StackCompleter;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackCompletion
    {
        public event Action<HexagonStackScore> StackCompleted;
        public event Action DeleteAnimationCompleted;

        private readonly GameplayAnimations _gameplayAnimations;
        private int _hexagonsCountForComplete;

        public StackCompletion(int hexagonsCountForComplete)
        {
            _hexagonsCountForComplete = hexagonsCountForComplete;
            _gameplayAnimations = new GameplayAnimations();
        }

        public IEnumerator CheckStackForCompleteRoutine(MergeCandidate mergeCandidate)
        {
            if (CanComplete(mergeCandidate.Stack, out List<Hexagon> similarHexagons) == false)
                yield break;

            HexagonTileType topHexagonTileType = mergeCandidate.Stack.TopHexagon.TileType;
            
            Tween deleteAnimation = DeleteAnimation(similarHexagons);
            yield return deleteAnimation.WaitForCompletion();

            int score = HexagonStackUtils.CalculateScore(similarHexagons);
            StackCompletePopUp(mergeCandidate);

            DeleteHexagons(mergeCandidate.Stack, similarHexagons);

            if (mergeCandidate.Stack.Hexagons.Count <= 0)
            {
                DeleteStack(mergeCandidate);
                
            }

            StackCompleted?.Invoke(new HexagonStackScore(topHexagonTileType, score));
        }

        private bool CanComplete(HexagonStack stack, out List<Hexagon> similarHexagons)
        {
            similarHexagons = null;
            
            if (stack.Hexagons.Count < _hexagonsCountForComplete)
                return false;

            HexagonTileType topHexagonTileType = stack.TopHexagon.TileType;
            
            similarHexagons = HexagonStackUtils.GetSimilarHexagons(stack,
                topHexagonTileType);
            
            return similarHexagons.Count >= _hexagonsCountForComplete;
        }
        
        private Tween DeleteAnimation(List<Hexagon> similarHexagons)
        {
            float delay = 0;
            Tween deleteAnimation = null;
            
            foreach (Hexagon hexagon in similarHexagons)
            {
                deleteAnimation =
                    _gameplayAnimations.HexagonDeleteAnimation(hexagon, delay, 0.2f, DeleteAnimationCompleted);

                deleteAnimation.Play();
                delay += 0.03f;
            }

            return deleteAnimation;
        }

        private void DeleteStack(MergeCandidate mergeCandidate)
        {
            mergeCandidate.Cell.FreeCell();

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