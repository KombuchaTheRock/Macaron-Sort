using System;
using UnityEngine;

namespace Sources.Common.CodeBase.Services.PlayerProgress.Data
{
    [Serializable]
    public class CellData
    {
        [field: SerializeField] public Vector2Int PositionOnGrid { get; private set; }

        public CellData(Vector2Int positionOnGrid)
        {
            PositionOnGrid = positionOnGrid;
        }
    }
}