using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDefinition {
    public Vector2Int dimensions;
    public BlockType[] blockTypeSerializedGrid;

    public int CalculateLinearizedCoordinates(int x, int y)
    {
        return CalculateLinearizedCoordinates(x, y, dimensions.x);
    }

    public static int CalculateLinearizedCoordinates(int x, int y, int width)
    {
        return y * width + x;
    }
}
