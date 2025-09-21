using System;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator MergeRoutine(StackMergeCandidate mergeCandidate, List<Hexagon> hexagonForMerge)
        {
            float offsetBetweenTiles = mergeCandidate.Stack.OffsetBetweenTiles;
            float initialY = (mergeCandidate.Stack.Hexagons.Count + 1) * offsetBetweenTiles;

            for (int i = 0; i < hexagonForMerge.Count; i++)
            {
                Hexagon hexagon = hexagonForMerge[i];

                float targetY = initialY + i * offsetBetweenTiles;

                mergeCandidate.Stack.Add(hexagon);
                hexagon.SetParent(mergeCandidate.Stack.transform);

                Tween mergeAnim = HexagonMergeAnimation(mergeCandidate.Stack, targetY, hexagon, 0.1f);
                mergeAnim.Play();
                
                yield return new WaitForSeconds(mergeAnim.Duration());
            }

            yield return _coroutineRunner.StartCoroutine(CheckStackForComplete(mergeCandidate));
        }

        private IEnumerator CheckStackForComplete(StackMergeCandidate mergeCandidate)
        {
            if (mergeCandidate.Stack.Hexagons.Count < HexagonsCountForComplete)
                yield break;

            List<Hexagon> similarHexagons = GetSimilarHexagons(mergeCandidate.Stack, mergeCandidate.Stack.TopHexagon, out bool isMonoType);

            if (isMonoType == false || similarHexagons.Count < HexagonsCountForComplete)
                yield break;

            while (similarHexagons.Count > 0)
            {
                HexagonDeleteAnimation(similarHexagons[0]);

                yield return new WaitForSeconds(0.1f);
                
                mergeCandidate.Stack.Remove(similarHexagons[0]);
                Object.Destroy(similarHexagons[0].gameObject);
                similarHexagons.RemoveAt(0);
            }

            mergeCandidate.Cell.SetStack(null);
        }

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
        
        private TweenerCore<Vector3, Vector3, VectorOptions> HexagonMergeAnimation(HexagonStack stack, float targetY, Hexagon hexagon, float duration)
        {
            Vector3 targetPosition = new(stack.transform.position.x, targetY, stack.transform.position.z);

            return hexagon.transform.DOMove(targetPosition, duration);
        }
        
        private void HexagonDeleteAnimation(Hexagon hexagon)
        {
            hexagon.transform.DOScale(0f, 0.2f)
                .Play()
                .SetLink(hexagon.gameObject);
        }
    }
}