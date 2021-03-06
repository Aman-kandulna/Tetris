using UnityEngine.Tilemaps;
using UnityEngine;
public enum Tetromino
{
    I,
    O,
    J,
    T,
    L,
    S,
    Z

}
[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        cells = Data.Cells[tetromino];
    }

}
