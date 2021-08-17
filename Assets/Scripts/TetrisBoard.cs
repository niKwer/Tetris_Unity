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
        ShapeData shape = this.shapes[random];
        this.currentTetromino.Initialize(spawnPosition, shape, this);
        Set(this.currentTetromino);
    }

    private void Set(Tetromino tetromino)
    {
        for (int i = 0; i < tetromino.cells.Length; i++)
        {
            Vector3Int tilePosition = tetromino.cells[i] + tetromino.position;
            this.tilemap.SetTile(tilePosition, tetromino.shape.tile);
        }
    }
}
