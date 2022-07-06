using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    public   List<Tile> tiles = new List<Tile>();
    string t;
    public  Tile GetTile(string name)
    {
        return tiles.Find(x => x.name == name);
    }
    private JSONUtility jsonUtility = new JSONUtility();
  /*  public void Start()
    {
        CookingTime cookingTime = jsonUtility.LoadJsonData<CookingTime>("Resources/CookingTime.json");
        Debug.Log(cookingTime.GetCookingTime("orange"));
      
    }*/
}
