using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimerManager : MonoBehaviour
{
    public GameObject timerPrefab;
    private int TIMERPOSITION_Y = -21; // in world space not in matrix space or tilemap space
    private int COLOUMN_WIDTH = 6; // in world space
    public static TimerManager instance;
    private GameObject[] timers;
    private void Awake()
    {
        if(instance !=null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
       timers = new GameObject[LevelEditor.Instance.panCount];
    }

    public Timer SpawnTimer(int coloumnNo, float countDownTime)
    {
       
        int x = (coloumnNo * COLOUMN_WIDTH) + COLOUMN_WIDTH / 2;
        int y = TIMERPOSITION_Y;
        int z = 0;
        GameObject t = Instantiate(timerPrefab, new Vector3(x, y, z),Quaternion.identity);
        timers[coloumnNo] = t;
        t.GetComponentInChildren<Timer>().Init(coloumnNo,countDownTime);
        return t.GetComponentInChildren<Timer>();
        
    }
    public Timer GetTimerAt(int coloumnNo)
    {
        return timers[coloumnNo].GetComponentInChildren<Timer>();   
    }
  
    public bool IsEmpty(int coloumnNo)
    {
        if(timers[coloumnNo] == null)
        {
            return true;
        }
        return false;
    }
    public void DestroyTimer(Timer timer)
    {
        timers[timer.GetColoumnNo()] = null;
        Transform temp = timer.gameObject.transform.parent;
        Destroy(temp.gameObject);
        
    }
}
