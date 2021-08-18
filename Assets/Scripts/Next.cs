using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Next : MonoBehaviour
{
    public Tile tile;
    public TetrisBoard board;
    public Tilemap tilemap { get; private set; }
    [SerializeField] public Vector3Int position;
    public Vector3Int[] cells { get; private set; }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }
    public void InitializeNextTetromino(ShapeData shapeData, TetrisBoard board)
    {
        Clear();
        this.tile = shapeData.tile;
        this.board = board;
        if (this.cells == null)
            this.cells = new Vector3Int[shapeData.cells.Length];

        for (int i = 0; i < shapeData.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)shapeData.cells[i];
        }
        Set();

    }
    private void Clear()
    {
        for (int x = -1; x <= 2; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, this.position.z) + this.position;
                this.tilemap.SetTile(tilePosition, null);
            }
        }
    }
    private void Set()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }

}
