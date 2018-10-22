using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    
    [SerializeField]
    private float timer;
    public Text ForTimeTextUI;

    void Awake()
    {
        _instance = this;
    }

    public static TimeManager GetInstance()
    {
        return _instance;
    }

    public void SetTimeText(string newMessage)
    {
        ForTimeTextUI.text = "Time : " +  newMessage;
    }

    public float GetTimer()
    {
        return timer;
    }

    public void SetTimer(float minusTimer)
    {
        if (timer > 0)
        {
            timer -= minusTimer;
        }
        else
        {
            timer = 0;
            GameSceneManager.GetInstance().SetGameEnd(true);
        }
    }
}
