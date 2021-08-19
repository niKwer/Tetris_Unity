using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct ShapeData
{
    public Vector2Int[,] wallKicks { get; private set; }
    public Vector2Int[] cells;
    public Shape shape;
    public Tile tile;

    public void InitializeWallKicks()
    {
        this.wallKicks = WallKicksData.WallKicks[this.shape];
    }
}
public enum Shape
{
    S,
    Z,
    L,
    J,
    T,
    O,
    I
}