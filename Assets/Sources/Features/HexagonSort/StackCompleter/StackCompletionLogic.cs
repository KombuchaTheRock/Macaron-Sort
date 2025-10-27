using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using Sources.Features.HexagonSort.Merge.Scripts;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Features.HexagonSort.StackCompleter
{
    public class StackCompletionLogic : IStackCompletionLogic
    {
        public event Action<HexagonStackScore> StackCompleted;
        public event Action DeleteAnimationCompleted;
    
        private readonly ICoroutineRunner _coroutineRunner;

        private readonly HexagonDeleteAnimation _completeAnimation = new();

        public IEnumerator CompleteStackRoutine(HexagonStack stack, GridCell gridCell)
        {
            HexagonTileType topHexagonTileType = stack.TopHexagon.TileType;
            
            Tween deleteAnimation = DeleteAnimation(stack.Hexagons.ToList());
            yield return deleteAnimation.WaitForCompletion();

            int score = HexagonStackUtils.CalculateScore(stack.Hexagons.ToList());
        
            StackCompletePopUp(stack, gridCell);
            DeleteStack(stack, gridCell);

            StackCompleted?.Invoke(new HexagonStackScore(topHexagonTileType, score));
        }

        private void DeleteStack(HexagonStack stack, GridCell gridCell)
        {
            foreach (Hexagon hexagon in stack.Hexagons.ToList())
            {
                stack.Remove(hexagon);
                Object.Destroy(hexagon.gameObject);
            }

            gridCell.FreeCell();
        }

        private Tween DeleteAnimation(List<Hexagon> hexagons)
        {
            float delay = 0;
            Tween deleteAnimation = null;

            hexagons.Reverse();
        
            foreach (Hexagon hexagon in hexagons)
            {
                deleteAnimation =
                    _completeAnimation.DeleteAnimation(hexagon, delay, 0.2f, DeleteAnimationCompleted);

                deleteAnimation.Play();
                delay += 0.03f;
            }

            return deleteAnimation;
        }

        private static void StackCompletePopUp(HexagonStack stack, GridCell cell)
        {
            Vector3 popUpPosition = stack.TopHexagon.transform.position;

            GameObject popUp = Resources.Load<GameObject>("StackGenerator/Prefab/StackCountPopUp");

            TextMeshPro popUpText = popUp.GetComponentInChildren<TextMeshPro>();
            popUpText.text = stack.Hexagons.Count.ToString();

            Object.Instantiate(popUp, popUpPosition, popUp.transform.rotation, cell.transform);
        }
    }
}