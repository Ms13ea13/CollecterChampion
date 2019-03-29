using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSceneManager : MonoBehaviour
{
    public static string targetPicture;

    [SerializeField]
    private int minFoodProvideID;

    [SerializeField]
    private int maxFoodProvideID;

    [SerializeField]
    private PlayerBase[] players;

    private TimeManager timeManager;

    [SerializeField]
    private EndPanel m_EndPanel;

    [SerializeField]
    private ScoreManager scoreManager;
    
    //Struct Area----------------------------------------------------------------------

    [SerializeField]
    private FoodNameOrder foodNameOrder;
    
    //Instance Area----------------------------------------------------------------------

    private static GameSceneManager _instance;
    public static GameSceneManager GetInstance()
    {
        return _instance;
    }


    //Game Area----------------------------------------------------------------------

    void Awake()
    {
        Initialized();
    }

    void Update()
    {
        TimeManageMent();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!OnOffPanel)
            {
                OnOffPanel = true;
                Time.timeScale = 0;
            }
            else
            {
                OnOffPanel = false;
                Time.timeScale = 1;
            }

            PausePanel.SetActive(OnOffPanel);
        }
    }

    //Private Area----------------------------------------------------------------------

    void TimeManageMent()
    {
        timeManager.SetTimer(Time.deltaTime);
        int temp = (int)timeManager.GetTimer();
        timeManager.SetTimeText(temp.ToString());
    }

    void Initialized()
    {
        _instance = this;
        timeManager = TimeManager.GetInstance();
        if (timeManager)
        timeManager.SetTimeText(TimeManager.GetInstance().GetTimer().ToString());
        else throw new Exception("TimeManager is null");
    }

    public void GameInitiate()
    {
        CustomerManager.GetInstance().Initiate();
    }

    //Public static Area----------------------------------------------------------------------

    public static void UpdateTargetPicture(string _wantTag)
    {
        targetPicture = _wantTag;
    }

    //Public Area----------------------------------------------------------------------

    public void SetGameEnd(bool checkEnd)
    {
        Time.timeScale = 0;
        m_EndPanel.gameObject.SetActive(checkEnd);
        //m_EndPanel.SettingEndPanel(players);
    }

    public string GetFoodNameById(int id)
    {
        return foodNameOrder.GetStringFoodName(id);
    }

    public Sprite GetFoodPictureById(int id)
    {
        return foodNameOrder.GetFoodPicture(id);
    }

    public int GetFoodPriceById(int id)
    {
        return foodNameOrder.GetFoodPrice(id);
    }

    public FoodType.FoodItemType GetFoodTypeById(int id)
    {
      
        return (FoodType.FoodItemType)id;
    }

    public int RandomFoodOrderID()
    {
        int rd = UnityEngine.Random.Range(minFoodProvideID, maxFoodProvideID);
        return rd;
    }

    public void CustomerPayMoneyToStore(int price)
    {
        scoreManager.AddScoreNumber(price);
    }

    public void CustomerDidNotPayMoneyToStore(int price)
    {
        scoreManager.RemoveScoreNumber(price);
    }

    //Pause Panel -------------------------------------------------------------------
    public GameObject PausePanel;

    void Start()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    bool OnOffPanel;
}

[Serializable]
public struct FoodNameOrder
{
    [SerializeField]
    private string[] orderName;

    [SerializeField]    
    private Sprite[] foodPicture;

    [SerializeField]
    private int[] price;
    
    public string GetStringFoodName(int id)
    {
        return orderName[id];
    }
    
    public Sprite GetFoodPicture(int id)
    {
        return foodPicture[id];
    }

    public int GetFoodPrice(int id)
    {
        return price[id];
    }
}

