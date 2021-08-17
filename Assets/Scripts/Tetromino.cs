using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public ShapeData shape { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public TetrisBoard board { get; private set; }
    public void Initialize(Vector3Int position, ShapeData shape, TetrisBoard board)
    {
        this.shape = shape;
        this.board = board;
        this.position = position;

        if (this.cells == null)
            this.cells = new Vector3Int[shape.cells.Length];

        for (int i = 0; i < shape.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)shape.cells[i];
        }
    }
}
