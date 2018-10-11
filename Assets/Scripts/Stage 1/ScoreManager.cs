using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    public Text ForScoreTextUI;

    void Start()
    {
        ForScoreTextUI.text = "Score : " + score;
    }

    void Update()
    {
        ForScoreTextUI.text = "Score : " + score;
    }
}
