using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    public GameObject[] gameEndPanelStar;

    private int collectStarCount = 0;
    private const int maxStarCollect = 3;

    public void ResetStars()
    {
        collectStarCount = 0;

        for (int i = 0; i < 3; i++)
            gameEndPanelStar[i].SetActive(false);
    }

    public void StarCollect(int star)
    {
        if (collectStarCount > maxStarCollect - 1)
            return;

        collectStarCount += star;
        if (star == 0)
        {
            gameEndPanelStar[0].SetActive(true);
        }
        else if (star == 1)
        {
            gameEndPanelStar[0].SetActive(true);
            gameEndPanelStar[1].SetActive(true);
        }
        else if (star == 2)
        {
            gameEndPanelStar[0].SetActive(true);
            gameEndPanelStar[1].SetActive(true);
            gameEndPanelStar[2].SetActive(true);
        }
    }
}
