using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TetrisBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tetromino currentTetromino { get; private set; }
    public ShapeData[] shapes;
    private Next nextBoard;
    [SerializeField]  public GameSession gameSession;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    private int nextValue;
    private bool isGameOver = false;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }
    public void Awake()
    {
        this.isGameOver = false;
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.currentTetromino = GetComponentInChildren<Tetromino>();
        this.gameSession = FindObjectOfType<GameSession>();
        this.nextBoard = FindObjectOfType<Next>();

        for (int i = 0; i < this.shapes.Length; i++)
        {
            this.shapes[i].InitializeWallKicks();
        }
    }
    private void Start()
    {
        SetAllStaticTetrominoes();
        FirstSpawn();
    }

    private void FirstSpawn()
    {
        ShapeData shapeData = this.shapes[RandomValue()];
        this.currentTetromino.Initialize(spawnPosition, shapeData, this);
        gameSession.AddCurrentTetromino(shapeData);

        nextValue = RandomValue();
        nextBoard.InitializeNextTetromino(this.shapes[nextValue], this);
    }
    private int RandomValue()
    {
        return Random.Range(0, this.shapes.Length);
    }
    public void SpawnTetromino()
    {
        ShapeData shapeData = this.shapes[nextValue];
        this.currentTetromino.Initialize(spawnPosition, shapeData, this);

        if(IsValidPosition(this.currentTetromino,this.spawnPosition) && !IsGameOver())
        {
            gameSession.AddCurrentTetromino(shapeData);
            SetTetromino(this.currentTetromino);
            nextValue = Random.Range(0, this.shapes.Length);
            nextBoard.InitializeNextTetromino(this.shapes[nextValue], this);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        if(!isGameOver)
            FindObjectOfType<AudioManager>().Play("GameOver");
        isGameOver = true;
        gameSession.GameOver();
    }

    public void SetTetromino(Tetromino tetromino)
    {
        if(!isGameOver)
        {
            for (int i = 0; i < tetromino.cells.Length; i++)
            {
                Vector3Int tilePosition = tetromino.cells[i] + tetromino.position;
                this.tilemap.SetTile(tilePosition, tetromino.shapeData.tile);
            }
        }
    }

    public void Clear(Tetromino tetromino)
    {
        if(!isGameOver)
        {
            for (int i = 0; i < tetromino.cells.Length; i++)
            {
                Vector3Int tilePosition = tetromino.cells[i] + tetromino.position;
                this.tilemap.SetTile(tilePosition, null);
            }
        }
    }

    public bool IsValidPosition(Tetromino tetromino, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < tetromino.cells.Length; i++)
        {
            Vector3Int tilePosition = tetromino.cells[i] + position;

            if(!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
            if(this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int countCleaningLines = 0;
        while(row<bounds.yMax)
        {
            if(IsLineFull(row))
            {
                countCleaningLines++;
                LineClear(row);
            }
            else
            {
                row++;
            }
            if (countCleaningLines > 0) { FindObjectOfType<AudioManager>().Play("Clear"); }
        }
        gameSession.AddScore(countCleaningLines);
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for(int col=bounds.xMin; col<bounds.xMax;col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }
    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position, null);
        }
        
        while(row<bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, above);
            }

            row++;
        }
    }

    private void SetAllStaticTetrominoes()
    {
        Vector3Int position = new Vector3Int(-14, 7, 0);
        for (int i = 0; i < shapes.Length; i++)
        {
            for (int j = 0; j < shapes[i].cells.Length; j++)
            {
                Vector3Int tilePosition = (Vector3Int)shapes[i].cells[j] + position;
                this.tilemap.SetTile(tilePosition, shapes[i].tile);
            }
            position.y -= 3;
        }
    }

    public bool IsGameOver() => isGameOver;
}
