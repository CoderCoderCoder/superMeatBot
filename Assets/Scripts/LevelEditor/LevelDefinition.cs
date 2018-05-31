using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDefinition {
    public Vector2Int dimensions;
    public BlockType[] blockTypeSerializedGrid;
}
