using System;
using System.Collections.Generic;
using NaughtyAttributes;
using Sources.Features.HexagonSort.HexagonTile.Scripts;
using UnityEngine;

[CreateAssetMenu(menuName = "StaticData/GridModifierConfig", fileName = "GridModifierConfig", order = 0)]
public class GridModifierConfig : ScriptableObject
{
    [field: SerializeField] public List<GridModifier> Modifiers { get; set; } = new();
}

[Serializable]
public class CellBlockerWithCondition
{
    [field: SerializeField, BoxGroup,
            Label("Hexagon Type"),]
    public HexagonTileType HexagonType { get; private set; }

    [field: SerializeField, Range(0, 100), BoxGroup]
    public int ScoreAmount { get; private set; }
}

[Serializable]
public class GridModifier
{
    [field: SerializeField] public int LevelForStart { get; private set; }

    // [field: SerializeField, Header("Modifiers")]
    // public bool AddNewCellsAdder { get; private set; }
    //
    // [field: SerializeField, MinMaxSlider(0, 10),
    //         ShowIf("AddNewCellsAdder"),
    //         Label("Min-Max New Cells Count")]
    // public Vector2Int MinMaxAddedCellsCount { get; private set; }
    //
    // [field: SerializeField, Space(5)] public bool AddCellsDeleter { get; private set; }
    //
    // [field: SerializeField, MinMaxSlider(0, 10),
    //         ShowIf("AddCellsDeleter"),
    //         Label("Min-Max Deleted Cells Count")]
    // public Vector2Int MinMaxDeletedCellsCount { get; private set; }
    //
    // [field: SerializeField, Space(5)] public bool AddSimpleBlocker { get; private set; }
    //
    // [field: SerializeField, MinMaxSlider(0, 10),
    //         ShowIf("AddSimpleBlocker"),
    //         Label("Blocker Count Range")]
    // public Vector2Int MinMaxSimpleBlockerCount { get; private set; }
    //
    // [field: SerializeField, Space(5)] public bool AddBlockerWithCondition { get; private set; }
    //
    // [field: SerializeField, ShowIf("AddBlockerWithCondition"), BoxGroup]
    // public List<CellBlockerWithCondition> BlockersWithCondition { get; private set; }
}