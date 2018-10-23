using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private PlayerBase[] players;

    private TimeManager timeManager;

    [SerializeField]
    private EndPanel m_EndPanel;
    
    public Image targetObj;

    [SerializeField]
    private SpawnItem spawnItem;
    
    [SerializeField]
    private string setTargetPicture;
    public static string targetPicture;

    [SerializeField]
    private FoodNameOrder foodNmaeOrder;

    
    private static GameSceneManager _instance;
    public static GameSceneManager GetInstance()
    {
        return _instance;
    }

   
    void Awake ()
    {
        _instance = this;
        targetPicture = "Item1";
        timeManager = TimeManager.GetInstance();
        timeManager.SetTimeText(TimeManager.GetInstance().GetTimer().ToString());
    }

    void Update()
    {
        setTargetPicture = targetPicture;
        timeManager.SetTimer(Time.deltaTime);
        int temp = (int)timeManager.GetTimer();
        timeManager.SetTimeText(temp.ToString());
    }

    public Sprite GetItemPicture(int id)
    {
        return spawnItem.GetItemPicture(id);
    }
    
    public static void UpdateTargetPicture(string _wantTag)
    {
        targetPicture = _wantTag;
    }

    public void UpdatePicture(Sprite pic)
    {
        targetObj.sprite = pic;
    }
    
    public void SetGameEnd(bool checkEnd)
    {
        Time.timeScale = 0;
        m_EndPanel.gameObject.SetActive(checkEnd);
        m_EndPanel.SettingEndPanel(players);
    }

    public string GetFoodNameById(int id)
    {
        return foodNmaeOrder.GetStringFoodName(id);
    }

    public Sprite GetFoodPictureById(int id)
    {
        return foodNmaeOrder.GetFoodPicture(id);
    }
}

[Serializable]
public struct FoodNameOrder
{
    [SerializeField]
    private string[] orderName;
    [SerializeField]    
    private Sprite[] foodPicture;
    
    public string GetStringFoodName(int id)
    {
        return orderName[id];
    }
    
    public Sprite GetFoodPicture(int id)
    {
        return foodPicture[id];
    }
}

