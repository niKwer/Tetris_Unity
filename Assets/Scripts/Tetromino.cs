using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public ShapeData shapeData { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public TetrisBoard board { get; private set; }
    public int rotationIndex { get; private set; }
    public void Initialize(Vector3Int position, ShapeData shapeData, TetrisBoard board)
    {
        this.shapeData = shapeData;
        this.board = board;
        this.position = position;
        this.rotationIndex = 0;

        if (this.cells == null)
            this.cells = new Vector3Int[shapeData.cells.Length];

        for (int i = 0; i < shapeData.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)shapeData.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);
        if(Input.GetButtonDown("Rotate"))
        {
            Rotate(1);
        }
        if (Input.GetButtonDown("MoveRight"))
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetButtonDown("MoveLeft"))
        {
            Move(Vector2Int.left);
        }

        if (Input.GetButtonDown("MoveDown"))
        {
            Move(Vector2Int.down);
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }
        this.board.SetTetromino(this);
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        RotationMethod(direction);
    }

    private void RotationMethod(int direction)
    {
        float cos = Mathf.Cos(Mathf.PI / 2f);
        float sin = Mathf.Sin(Mathf.PI / 2f);
        float[] matrix = new float[] { cos, sin, -sin, cos };

        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.shapeData.shape)
            {
                case Shape.I:
                case Shape.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * matrix[0] * direction) + (cell.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * matrix[2] * direction) + (cell.y * matrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }

    private bool Move(Vector2Int direction)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += direction.x;
        newPosition.y += direction.y;

        bool isValid = this.board.IsValidPosition(this, newPosition);

        if (isValid)
        {
            this.position = newPosition;
        }

        return isValid;
    }
    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
    }
}
