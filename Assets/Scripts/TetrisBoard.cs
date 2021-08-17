using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TetrisBoard : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public ShapeData[] shapes;

    private void Start()
    {
        SpawnShape();
    }

    private void SpawnShape()
    {
        int random = Random.Range(0, this.shapes.Length);
        ShapeData data = this.shapes[random];
    }

    private void Set()
    {

    }
}
