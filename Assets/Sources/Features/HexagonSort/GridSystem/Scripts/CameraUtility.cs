using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Sources.Features.HexagonSort.GridSystem.Scripts
{
    public static class CameraUtility
    {
        public static void UpdateCameraSize(HexagonGrid grid)
        {
            float maxGridMagnitude = grid.Cells
                .Select(cell => cell.transform.position.magnitude)
                .DefaultIfEmpty(0f)
                .Max();

            float targetSize = Mathf.Max(5, maxGridMagnitude * 2.57f);

            DOTween.To(
                    () => Camera.main.orthographicSize,
                    x => Camera.main.orthographicSize = x,
                    targetSize,
                    0.3f
                )
                .SetLink(Camera.main.gameObject)
                .Play();
        }
    }
}