using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private AudioSource[] stopSound;

    public GameObject[] disableOBJ;

    void Update()
    {
        disableOBJ[0].SetActive(false);
        disableOBJ[1].SetActive(false);

        stopSound[0].Stop();
        stopSound[1].Stop();
    }

    /*[SerializeField]
    private Text[] scoreShow;

    [SerializeField]
    private Image[] avartarShow;

    [SerializeField]
    private Sprite[] playerAvatar;

    [SerializeField]
    private int winnerID;
    int tempI = 1;
      
    public void SettingEndPanel(PlayerBase[] players)
    {
        int winner = 0;
        int temp;
        
        for (int i = 0; i < players.Length; i++)
        {
            temp = players[i].GetScore();
            
            if (temp > winner)
            {
                winner = temp;
                winnerID = i;
            }
        }
     
        scoreShow[0].text = "Player " + players[winnerID].GetIdPlayer() +" : " + winner.ToString();
        avartarShow[0].sprite = playerAvatar[winnerID];

        //SetShow
        //scoreShow[i].text = "Player " + i + " : " + players[i].GetScore().ToString();
        //avartarShow[i].sprite = playerAvatar[i];

        //SetHide
        //scoreShow[i].text = "Player " + i;
        //Color tempColor = avartarShow[i].color;
        //tempColor.a = .2f;
        //avartarShow[i].color = tempColor;

        for (int j = 0; j < players.Length; j++)
        {
            if (j != winnerID && tempI < players.Length)
            {
                scoreShow[tempI].text = "Player " + players[j].GetIdPlayer() +" : " + players[j].GetScore().ToString();
                avartarShow[tempI].sprite = playerAvatar[j];
                tempI += 1;
            }
            else if (tempI >= players.Length)
            {
                if (tempI < scoreShow.Length)
                {
                    scoreShow[tempI].text = "Player " + tempI;
                    Color tempColor = avartarShow[tempI].color;
                    tempColor.a = .2f;
                    avartarShow[tempI].color = tempColor;
                }
            }
        }
    }*/
}
