using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer:MonoBehaviour
{
    float countDownTime;
    Image fill;
    Slider slider;
    int coloumnNo;
    
    public int GetColoumnNo()
    {
        return coloumnNo;
    }
    public void Init(int coloumnNo,float countDownTime)
    {
        this.countDownTime = countDownTime;
        slider = GetComponent<Slider>();
        slider.maxValue = countDownTime;
        fill = transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        fill.color = Color.green;
        this.coloumnNo = coloumnNo;
    }
    public void StartTimer()
    {
      
        if(countDownTime > 0)
        {
           StartCoroutine(LoopTimer());
        }
    }
   
    public void IncreaseCountDownTime(int timeInterval)
    {
        
        float x = countDownTime + timeInterval;
        if(x > slider.maxValue)
        {
            slider.maxValue = x;
        }
        countDownTime = x;
    }
    IEnumerator LoopTimer()
    {
        while (countDownTime > 0)
        {
            countDownTime -= Time.deltaTime;
            slider.value = countDownTime;
            fill.color = Color.Lerp(fill.color, Color.red, Time.deltaTime);
            yield return null;
        }
        TimerManager.instance.DestroyTimer(this);
    }
   

}
