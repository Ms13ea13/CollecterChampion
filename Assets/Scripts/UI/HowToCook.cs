using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToCook : MonoBehaviour
{
    [SerializeField]
    private GameObject howToCook;

    private static bool ShowTutorial;

    void Start()
    {
        Time.timeScale = 0;
        howToCook.SetActive(true);
        ShowTutorial = true;
    }

    void Update()
    {
        if (Input.anyKey && ShowTutorial)
        {
            if (Time.timeScale == 0)
                Time.timeScale = 1;

            ShowTutorial = false;
            howToCook.SetActive(false);
            GameSceneManager.GetInstance().GameInitiate();
        }
    }

    public static bool IsShowingTutorial()
    {
        return ShowTutorial;
    }
}
