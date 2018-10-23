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

    /*public string GetStringSomething(int id)
    {
        if (id == 1)
        {
            return "sus";
        }
        else
        {
            return ":";
        }
    }*/
}

[Serializable]
public struct FoodNameOrder
{
    public string GetStringFoodName(int id)
    {
        switch (id)
        {
            case 1 :
                return "rice";
            case 2 :
                return "egg";
            case 3 :
                return "steak";
            case 4 :
                return "hambergur";
            case 5 :
                return "pizza";
            case 6 :
                return "cola";
            case 7 :
                return "sprite";
            case 8 :
                return "soda";
            case 9 :
                return "beer";
            default :
                return null;
        }
    }
}

