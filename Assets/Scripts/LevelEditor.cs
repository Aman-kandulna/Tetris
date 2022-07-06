using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public int panCount = 3;
    internal int PAN_WIDTH = 3;

    public int OrderCount = 8;
    public float stepDelay = 1.0f;

    [HideInInspector]
    public int MATRIX_ROW = 10;
    private int MATRIX_COLUMN;

    public static LevelEditor Instance
    {
        get;
        private set;
    }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
      
    }
 
}
