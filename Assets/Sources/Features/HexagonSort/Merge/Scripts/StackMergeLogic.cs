using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sources.Common.CodeBase.Infrastructure;
using Sources.Features.HexagonSort.GridSystem.GridGenerator.Scripts;
using Sources.Features.HexagonSort.GridSystem.Scripts;
using Sources.Features.HexagonSort.HexagonStackSystem.StackGenerator.Scripts;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Features.HexagonSort.Merge.Scripts
{
    public class StackMergeLogic
    {
        private const int HexagonsCountForComplete = 10;
        private readonly HexagonGrid _hexagonGrid;

        private Vector2Int[] _neighboursOddRow =
        {
            new(1, 0),
            new(-1, 0),
            new(0, 1),
            new(1, 1),
            new(0, -1),
            new(1, -1)
        };

        private Vector2Int[] _neighboursEvenRow =
        {
            new(1, 0),
            new(-1, 0),
            new(-1, 1),
            new(0, 1),
            new(-1, -1),
            new(0, -1)
        };

        private Coroutine _mergeRoutine;
        private ICoroutineRunner _coroutineRunner;

        public StackMergeLogic(HexagonGrid hexagonGrid, ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
            _hexagonGrid = hexagonGrid;
        }

        public List<GridCell> GetSimilarNeighbourCells(Vector2Int centerPosition, HexagonTileType topHexagonType)
        {
            List<GridCell> result = new();
            Vector2Int[] neighbours = centerPosition.y % 2 == 0 ? _neighboursEvenRow : _neighboursOddRow;

            foreach (Vector2Int offset in neighbours)
            {
                Vector2Int neighborPos = centerPosition + offset;

                if (_hexagonGrid.TryGetCell(neighborPos, out GridCell cell) == false)
                    continue;

                if (cell.IsOccupied && cell.Stack.TopHexagon == topHexagonType)
                    result.Add(cell);
            }

            return result;
        }

        public List<Hexagon> GetHexagonsToMerge(HexagonTileType topHexagon, HexagonStack stack)
        {
            List<Hexagon> hexagonToMerge = new();

            for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = stack.Hexagons[i];

                if (hexagon.TileType != topHexagon)
                    break;

                hexagonToMerge.Add(stack.Hexagons[i]);
                hexagon.SetParent(null);
            }

            return hexagonToMerge;
        }

        public void RemoveHexagonsFromStack(HexagonStack stack, List<Hexagon> hexagonToMerge)
        {
            foreach (Hexagon hexagon in hexagonToMerge)
            {
                if (stack.Contains(hexagon))
                    stack.Remove(hexagon);
            }
        }

        public Coroutine CheckStackForComplete(StackMergeCandidate mergeCandidate) =>
            _coroutineRunner.StartCoroutine(CheckStackForCompleteRoutine(mergeCandidate));

        private IEnumerator CheckStackForCompleteRoutine(StackMergeCandidate mergeCandidate)
        {
            Tween deleteAnimation = null;

            if (mergeCandidate.Stack.Hexagons.Count < HexagonsCountForComplete)
                yield break;

            List<Hexagon> similarHexagons = GetSimilarHexagons(mergeCandidate.Stack, mergeCandidate.Stack.TopHexagon,
                out bool isMonoType);

            if (isMonoType == false || similarHexagons.Count < HexagonsCountForComplete)
                yield break;

            float delay = 0;
            
            while (similarHexagons.Count > 0)
            {
                deleteAnimation = HexagonDeleteAnimation(similarHexagons.First(), delay, 0.2f);
                deleteAnimation.Play();

                delay += 0.01f;
                
                similarHexagons.RemoveAt(0);
            }

            yield return deleteAnimation.WaitForCompletion();

            mergeCandidate.Cell.SetStack(null);
            DeleteStack(mergeCandidate);
        }

        public IEnumerator MergeRoutine(StackMergeCandidate mergeCandidate, List<Hexagon> hexagonForMerge)
        {
            float offsetBetweenTiles = mergeCandidate.Stack.OffsetBetweenTiles;
            float initialY = mergeCandidate.Stack.Hexagons[^1].transform.position.y + offsetBetweenTiles;
            Tween currentAnimation = null;

            for (int i = 0; i < hexagonForMerge.Count; i++)
            {
                Hexagon hexagon = hexagonForMerge[i];

                float targetY = initialY + i * offsetBetweenTiles;

                mergeCandidate.Stack.Add(hexagon);
                hexagon.SetParent(mergeCandidate.Stack.transform);

                float duration = 0.2f;
                currentAnimation = HexagonMergeAnimation(mergeCandidate.Stack, targetY, hexagon, duration);
                currentAnimation.Play();
            }

            yield return currentAnimation.WaitForCompletion();
        }

        private void DeleteStack(StackMergeCandidate mergeCandidate) => 
            Object.Destroy(mergeCandidate.Stack.gameObject);

        public List<Hexagon> GetSimilarHexagons(HexagonStack stack, HexagonTileType sample, out bool isMonoType)
        {
            isMonoType = true;
            List<Hexagon> similarHexagons = new();

            for (int i = stack.Hexagons.Count - 1; i >= 0; i--)
            {
                Hexagon hexagon = stack.Hexagons[i];

                if (hexagon.TileType != sample)
                {
                    isMonoType = false;
                    break;
                }

                similarHexagons.Add(hexagon);
            }

            return similarHexagons;
        }

        private Tween HexagonMergeAnimation(HexagonStack stack, float targetY,
            Hexagon hexagon, float duration)
        {
            Vector3 targetPosition = new(stack.transform.position.x, targetY, stack.transform.position.z);
            Vector3 movementDirection = targetPosition - hexagon.transform.position;

            float delay = hexagon.transform.GetSiblingIndex() * 0.02f;

            Vector3 horizontalDir = new Vector3(movementDirection.x, 0f, movementDirection.z).normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, horizontalDir);

            Quaternion currentRotation = hexagon.transform.rotation;
            Quaternion targetRotation = Quaternion.AngleAxis(-180, rotationAxis) * currentRotation;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(hexagon.transform.DORotateQuaternion(targetRotation, duration)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay));

            sequence.Join(hexagon.transform.DOJump(targetPosition, 0.5f, 1, duration)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay));

            sequence.SetLink(hexagon.gameObject);

            return sequence;
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> HexagonDeleteAnimation(Hexagon hexagon, float delay,
            float duration)
        {
            return hexagon.transform.DOScale(0f, duration)
                .SetEase(Ease.InBack)
                .SetDelay(delay)
                .SetLink(hexagon.gameObject);
        }
    }
}