using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text activeOrderText;
    private string text = new string("Active Orders - ");
    public static UIManager Instance
    {
        get;
        private set;
    }
    public void Awake()
    {
        if(Instance !=null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void Start()
    {
        AddString("");
    }
    // recives recipe name and displays it as active order
    public void AddString(string message)
    {
        text +=message + "  ";
        activeOrderText.text = text.ToUpper();
    }
    //recives recipe name and removes it from active order
    public void RemoveString(string str)
    {
        text = text.Remove(text.IndexOf(str), str.Length + 2);
        activeOrderText.text = text.ToUpper();
       
    }
   
}
