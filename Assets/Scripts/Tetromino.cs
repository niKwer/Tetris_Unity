using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    public ShapeData shapeData { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public TetrisBoard board { get; private set; }
    public int rotationIndex { get; private set; }
    public AudioManager audioManager;

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;
    public void Initialize(Vector3Int position, ShapeData shapeData, TetrisBoard board)
    {
        this.shapeData = shapeData;
        this.board = board;
        this.position = position;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
            this.cells = new Vector3Int[shapeData.cells.Length];

        for (int i = 0; i < shapeData.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)shapeData.cells[i];
        }
    }
    private void Update()
    {
        this.lockTime += Time.deltaTime;
        if(!board.IsGameOver())
        {
            this.board.Clear(this);
            Movement();
        }
        if (Time.time > this.stepTime)
        {
            Step();
        }
        this.board.SetTetromino(this);
    }

    private void Movement()
    {
        if (Input.GetButtonDown("Rotate"))
        {
            Rotate(1);
        }
        if (Input.GetButtonDown("MoveRight"))
        {
            FindObjectOfType<AudioManager>().Play("Move");
            Move(Vector2Int.right);
        }
        else if (Input.GetButtonDown("MoveLeft"))
        {
            FindObjectOfType<AudioManager>().Play("Move");
            Move(Vector2Int.left);
        }

        if (Input.GetButtonDown("MoveDown"))
        {
            FindObjectOfType<AudioManager>().Play("Move");
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FindObjectOfType<AudioManager>().Play("Fall");
            HardDrop();
        }
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);
        RotationMethod(direction);
        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            RotationMethod(-direction);
        }
        FindObjectOfType<AudioManager>().Play("Rotate");
    }


    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.shapeData.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.shapeData.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationIndex < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0, this.shapeData.wallKicks.GetLength(0));
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
            this.lockTime = 0f;
        }

        return isValid;
    }
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);
        if (this.lockTime >= this.lockDelay && !Move(Vector2Int.down))
        {
            FindObjectOfType<AudioManager>().Play("Lock");
            Lock();
        }
    }

    private void Lock()
    {
        this.board.SetTetromino(this);
        this.board.ClearLines();
        this.board.SpawnTetromino();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }
}
