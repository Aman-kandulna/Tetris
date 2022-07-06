using UnityEngine;
using UnityEngine.Tilemaps;

public class Ingridient
{
    private Tile tile;
    private int Xpos;
    private int Ypos;
    private string name;

    public void Init(string name, int x, int y , Tile tile )
    {

        this.name = name;
        this.Xpos = x;
        this.Ypos = y;
        SetTile( tile );
    }
    public string GetName()
    {
        return name;
    }
    public void SetTile(Tile tile)
    {
        this.tile = tile;
       
    }
    public Tile GetTile()
    {
        return this.tile;
    }
    public void SetPosition(int x, int y)
    {
        Xpos = x;
        Ypos = y;
    }
    public int GetX()
    {
        return Xpos;
    }
    public int GetY()
    {
        return Ypos;
    }
  
  

}
