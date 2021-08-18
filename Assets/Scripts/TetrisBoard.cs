using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TetrisBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tetromino currentTetromino { get; private set; }
    public ShapeData[] shapes;
    public Next nextBoard;
    [SerializeField]  public GameSession gameSession;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    private int nextValue;

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
        FirstSpawn();
    }

    private void FirstSpawn()
    {
        int random = Random.Range(0, this.shapes.Length);
        ShapeData shapeData = this.shapes[random];
        this.currentTetromino.Initialize(spawnPosition, shapeData, this);

        nextValue = Random.Range(0, this.shapes.Length);
        nextBoard.InitializeNextTetromino(this.shapes[nextValue], this);
    }

    public void SpawnTetromino()
    {
        ShapeData shapeData = this.shapes[nextValue];
        this.currentTetromino.Initialize(spawnPosition, shapeData, this);

        if(IsValidPosition(this.currentTetromino,this.spawnPosition))
        {
            SetTetromino(this.currentTetromino);
        }
        else
        {
            GameOver();
        }
        nextValue= Random.Range(0, this.shapes.Length);
        nextBoard.InitializeNextTetromino(this.shapes[nextValue], this);
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }

    public void SetTetromino(Tetromino tetromino)
    {
        for (int i = 0; i < tetromino.cells.Length; i++)
        {
            Vector3Int tilePosition = tetromino.cells[i] + tetromino.position;
            this.tilemap.SetTile(tilePosition, tetromino.shapeData.tile);
        }
    }

    public void Clear(Tetromino tetromino)
    {
        for (int i = 0; i < tetromino.cells.Length; i++)
        {
            Vector3Int tilePosition = tetromino.cells[i] + tetromino.position;
            this.tilemap.SetTile(tilePosition, null);
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
}
