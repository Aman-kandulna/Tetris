using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Recipe
{
    public string name;
    public List<string> ingridients;
    public Recipe()
    {
        ingridients = new List<string>();
    }
    public int IngridientCount()
    {
        return ingridients.Count;
    }
    public List<string> GetIngridients()
    {
        return  ingridients;
    }
}


[System.Serializable]
public class RecipeList
{
    public List<Recipe> list;
    public int RecipeCount()
    {
        return list.Count;
    }
}

public class JSONUtility
{
    public t LoadJsonData<t>(string path)
    {
        string jsonString = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<t>(jsonString);
    }
    
}
public class ListUtility
{
    public bool ListComparer(List<string> list1, List<string> list2)
    {


        if (list1.Count != list2.Count)
            return false;
        List<string> temp1 = new List<string>(list1);
        List<string> temp2 = new List<string>(list2);
        temp1.Sort();
        temp2.Sort();

        for (int i = 0; i < temp1.Count; i++)
        {
            if (temp1[i] != temp2[i])
            {
                return false;
            }
        }

        return true;
    }
    public bool isEmpty<t>(List<t> list)
    {
        if (list.Count == 0)
            return true;
        return false;
    }
}
/*public class CookingTimes
{
    Dictionary<string, int> cookingTimeList;
    public void Init()
    {
        cookingTimeList = new Dictionary<string, int>();
        cookingTimeList.Add("apple", 5);
        cookingTimeList.Add("banana", 3);
        cookingTimeList.Add("orange", 4);
        cookingTimeList.Add("chocolate", 4);
        cookingTimeList.Add("curd", 2);
    }
    public int GetCookingTime(string ingridient)
    {
      
        return cookingTimeList[ingridient];
    }
}*/
[System.Serializable]
public class CookingTime
{
   public List<IngridientTimer> cookingTimeList;
   public int GetLength()
    {
        return cookingTimeList.Count;
    }
    public int GetCookingTime(string ingridient)
    {
        return cookingTimeList.Find(x => x.name == ingridient).time;
    }
}
[System.Serializable]
public class IngridientTimer
{
    public string name;
    public int time;

 

}
