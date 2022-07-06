using UnityEngine;
using System.Collections.Generic;
using System;
public class GameManager : MonoBehaviour
{

    private List<int> orders = new List<int>();
    private int maxOrders;
    private int ordernumber = -1;
    private List<Recipe> activeOrders = new List<Recipe>();
    private int maxActiveOrders;
    private RecipeList recipes;
    private List<string> activeOrderIngridients = new List<string>();

    public TileManager TileManager;
    public GameBoard gameboard;
    private Vector3 spawnPosition = Vector3.zero;
    private Ingridient activeIngridient;
   // private Ingridient[,] gamematrix = new Ingridient[ROW, COLOUMN];
    private List<List<Ingridient>> gamematrix = new List<List<Ingridient>>();
    private bool gameover;

    private int COLOUMN;
    private  int ROW = 10;
    private float stepDelay;
    private float stepTimer = 0.0f;
    private int panCount;

    private JSONUtility jsonUtility = new JSONUtility();
    private ListUtility listUtility = new ListUtility();
    private CookingTime cookingTime;
    private int PAN_WIDTH;
    
    private void Awake()
    {
        recipes = jsonUtility.LoadJsonData<RecipeList>("Resources/Recipes.json");
        cookingTime = jsonUtility.LoadJsonData<CookingTime>("Resources/CookingTime.json");
        NewGame();
    }
    private void Start()
    {
        SpawnIngridient();
       
    }
    private void Update()
    {
        if (!gameover)
        {
            stepTimer += Time.deltaTime;
            if (stepTimer > stepDelay)
            {
                stepTimer = 0;
                if (!isFalling(activeIngridient))
                {
                    
                    Recipe cookedRecipe = CheckRecipe(activeIngridient); // does the active piece end up making a recipe
                    if (cookedRecipe != null)
                    {
                        Debug.Log(cookedRecipe.name + " cooked ");
                        for (int i = 0; i < cookedRecipe.IngridientCount(); i++)
                        {
                            int m = activeIngridient.GetX();
                            int n = activeIngridient.GetY();
                            SetNull(m + i, n);
                        }

                        RemoveOrder(cookedRecipe);
                        AddNextOrder();
                        TimerManager.instance.DestroyTimer(TimerManager.instance.GetTimerAt((int)activeIngridient.GetY() / PAN_WIDTH));

                        if(CheckLevelClear())
                        {
                            Debug.Log("Level cleared");
                            gameover = true;
                        }
                    }
                    if(TimerManager.instance.IsEmpty((int)activeIngridient.GetY() / PAN_WIDTH)) // Coordinate system of timer
                    {
                        Timer t = TimerManager.instance.SpawnTimer((int)activeIngridient.GetY() / PAN_WIDTH, cookingTime.GetCookingTime(activeIngridient.GetName())); // pass in coloumn number
                        if (t != null)
                        {
                            t.StartTimer();
                        }
                    }
                    else
                    {
                       Timer t = TimerManager.instance.GetTimerAt((int)activeIngridient.GetY() / PAN_WIDTH);
                        t.IncreaseCountDownTime(cookingTime.GetCookingTime(activeIngridient.GetName()));
                    }
                 
                   
                    SpawnIngridient();
                }
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(activeIngridient, 0, -1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(activeIngridient, 0, 1);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardFall(activeIngridient);
            }
        }
        gameboard.Draw(gamematrix);
    }

    // spawns a new boardpiece at one of the spawnPosition
    private void SpawnIngridient()
    {
        if (activeOrderIngridients.Count != 0)
        {
            Ingridient piece = new Ingridient();
            string pieceName = activeOrderIngridients[UnityEngine.Random.Range(0, activeOrderIngridients.Count)];
            spawnPosition.y = UnityEngine.Random.Range(0, panCount) * PAN_WIDTH + 1; // 1 is an offset for when coloum is 0
       
            piece.Init(pieceName, (int)spawnPosition.x, (int)spawnPosition.y, TileManager.GetTile(pieceName));

            activeIngridient = piece;
            SetPosition(activeIngridient, activeIngridient.GetX(), activeIngridient.GetY());
         
        }

    }
    //checks if the boardpiece is falling or not
    private bool isFalling(Ingridient ingridient)
    {

        if (Move(ingridient, 1, 0))
        {
            return true;
        }
        return false;
    }
    private void HardFall(Ingridient ingridient)
    {
        while (Move(ingridient, 1, 0)) ;
    }
    // Returns true when piece can be moved in xoffset and yoffset else returns false
    private bool Move(Ingridient ingridient, int rowOffset, int coloumnOffset) // x and y are in matrix coordinates
    {
        
        if (IsValidPosition(ingridient.GetX() + rowOffset, ingridient.GetY() + (coloumnOffset * PAN_WIDTH)))
        {
            if (IsEmpty(ingridient.GetX() + rowOffset, ingridient.GetY() + (coloumnOffset * PAN_WIDTH)))
            {
                SetNull(ingridient.GetX(), ingridient.GetY());
                int x = ingridient.GetX() + rowOffset;
                int y = ingridient.GetY() +(coloumnOffset * PAN_WIDTH);
                ingridient.SetPosition(x, y);
                SetPosition(ingridient, ingridient.GetX(), ingridient.GetY());

                return true;
            }
            return false;
        }
        return false;
    }
    //sets position x and y to null on gameMatrix
    private void SetNull(int x, int y)
    {
        gamematrix[x][y] = null;
    }
    //sets piece at position x and y on matrix
    private void SetPosition(Ingridient ingridient, int x, int y)
    {
        gamematrix[x][y] = ingridient;
    }
    //Returns the recipe if piece completes a recipe else returns false
    private Recipe CheckRecipe(Ingridient ingridient)
    {
     
        Recipe cookedrecipe = new Recipe();      
        int x = 0;
        int y = 0;
        do
        {
            cookedrecipe.ingridients.Add(gamematrix[ingridient.GetX() + x][ingridient.GetY() + y].GetName() );
            x++;
        } while(IsValidPosition(ingridient.GetX() + x, ingridient.GetY() + y));
        

        List<string> list1 = new List<string>(cookedrecipe.ingridients);
        List<string> list2 = new List<string>();

        foreach (Recipe recipe in activeOrders)
        {
            list2 = recipe.GetIngridients();
            while (list1.Count > 0)
            {
                if (listUtility.ListComparer(list1, list2))
                {
                    return recipe;
                }

                list1.RemoveAt(list1.Count - 1);

            }
            list1 = new List<string>(cookedrecipe.ingridients);

        }
        return null;

    } 
    // Returns true when x and y are valid position on gameMatrix else returns false
    private bool IsValidPosition(int x, int y)
    {
        if (x < 0 || x >= ROW || y < 0 || y >= COLOUMN)
            return false;
        return true;
    }
    //Returns true if position x and y on gamematrix in empty else returns false
    private bool IsEmpty(int x, int y)
    {
        if (gamematrix[x][y] == null)
            return true;
        return false;
    }
    private void NewGame()
    {
        //Setup  necessary variables
        Init();

        //Setup necessary datastructures
        GenerateOrders(maxOrders);
        GenerateInitialActiveOrders();
        GenerateGameMatrix();
        ClearGameMatrix();
    }
    private void Init()
    {
        LevelEditor levelEditor = LevelEditor.Instance;

        gameover = false;
        panCount = levelEditor.panCount;
        PAN_WIDTH = levelEditor.PAN_WIDTH;
        maxOrders = levelEditor.OrderCount;
        maxActiveOrders = panCount;
        stepDelay = levelEditor.stepDelay;
        ROW = levelEditor.MATRIX_ROW;

    }
    private void ClearGameMatrix()
    {
        for (int i = 0; i < gamematrix.Count; i++)
        {
            for (int j = 0; j < gamematrix[0].Count; j++)
            {
                gamematrix[i][j] = null;
            }
        }
    }
    private void GenerateGameMatrix()
    {
        COLOUMN = panCount * PAN_WIDTH;
        for(int  i = 0; i< ROW; i++)
        {
            gamematrix.Add(new List<Ingridient>());
            for(int j = 0; j < COLOUMN; j++)
            {
                gamematrix[i].Add(new Ingridient());
            }
        }
    }
    private void GenerateOrders(int orderCount) 
    {
        for(int i = 0; i < orderCount; i++)
        {
            orders.Add(UnityEngine.Random.Range(0, recipes.RecipeCount()));
            
        }
    }
    private void GenerateInitialActiveOrders()
    {
        for(int  i = 0; i < maxActiveOrders; i++)
        {
            AddNextOrder();
        }
    }
    private void AddNextOrder()
    {
        if(++ordernumber < orders.Count)
        {
        activeOrders.Add(recipes.list[orders[ordernumber]]);
        AddActiveOrderIngridients(recipes.list[orders[ordernumber]]);
        UIManager.Instance.AddString(recipes.list[orders[ordernumber]].name);
        }
    }
    private void RemoveOrder(Recipe cookedRecipe)
    {
        activeOrders.Remove(activeOrders.Find(x => x.name == cookedRecipe.name));
        RemoveActiveOrderIngridients(cookedRecipe);
        UIManager.Instance.RemoveString(cookedRecipe.name);
    }
    private void AddActiveOrderIngridients(Recipe recipe)
    {
        activeOrderIngridients.AddRange(recipe.ingridients);
    }
    private void RemoveActiveOrderIngridients(Recipe recipe)
    {
        foreach (string ingridient in recipe.ingridients)
        {
            activeOrderIngridients.Remove(ingridient);
        }
    }
    private bool CheckLevelClear()
    {
        if (listUtility.isEmpty<Recipe>(activeOrders))
        {
            return true;
        }
        return false;
    }
  
}

