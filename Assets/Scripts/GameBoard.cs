using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard : MonoBehaviour
{
    private Tilemap tilemap;

    public void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
    }
    public void Draw(List<List<Ingridient>> gameMatrix)
    {

        for(int i= 0; i < gameMatrix.Count; i++)
        {
            for (int j = 0; j < gameMatrix[0].Count; j++)
            {
                if(gameMatrix[i][j] != null)
                {
                    //conversion from matrix coordinates to tilemap coordinates
                    tilemap.SetTile(new Vector3Int(gameMatrix[i][j].GetY(), -gameMatrix[i][j].GetX(), 0), gameMatrix[i][j].GetTile());
                }
                else
                {

                    tilemap.SetTile(new Vector3Int(j, -i, 0), null);
                }
            }
        }
        
    }
    
}
