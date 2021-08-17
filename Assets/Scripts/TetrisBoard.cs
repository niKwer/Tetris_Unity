using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TetrisBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tetromino currentTetromino { get; private set; }
    public ShapeData[] shapes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Start()
    {
        SpawnTetromino();
    }
    public void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.currentTetromino = GetComponentInChildren<Tetromino>();
    }
    private void SpawnTetromino()
    {
        int random = Random.Range(0, this.shapes.Length);
        ShapeData shapeData = this.shapes[random];
        this.currentTetromino.Initialize(spawnPosition, shapeData, this);
        SetTetromino(this.currentTetromino);
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
}
