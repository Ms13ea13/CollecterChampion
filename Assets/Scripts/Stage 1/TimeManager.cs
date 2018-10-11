using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static float time = 21;
    public Text ForTimeTextUI;

    void Start()
    {
        ForTimeTextUI.text = "Time : " + time;
    }

    void Update()
    {
        ForTimeTextUI.text = "Time : " + (int)time;

        if (time <= 0)
        {
            time = 0;
        }
    }
}
