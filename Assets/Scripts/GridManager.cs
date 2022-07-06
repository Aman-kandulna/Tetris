using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public SpriteRenderer grid;
    public SpriteRenderer border;
    private int ROW;
    private int COLOUMN;

    private void Start()
    {
        ROW = LevelEditor.Instance.MATRIX_ROW;
        COLOUMN = LevelEditor.Instance.panCount * LevelEditor.Instance.PAN_WIDTH;


        grid.drawMode = SpriteDrawMode.Tiled;
        grid.size = new Vector2(COLOUMN * 2, ROW * 2);

        Transform gridTransform = grid.GetComponent<Transform>();
        gridTransform.position = new Vector3(COLOUMN, -ROW, 0);

        Transform borderTransform = border.GetComponent<Transform>();
        borderTransform.position = new Vector3(COLOUMN, -ROW, 0);
        borderTransform.localScale = new Vector3(grid.size.x / 10, 1, 1);
    }
}
