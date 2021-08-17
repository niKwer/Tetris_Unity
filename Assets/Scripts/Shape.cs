using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct ShapeData
{
    public Vector2Int[] cells;
    public Shape shape;
    public Tile tile;
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