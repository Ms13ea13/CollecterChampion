using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColiderCheck : MonoBehaviour
{
    [SerializeField]
    private PlayerBase targetPlayer;

    [SerializeField]
    private Text ForScoreTextUI;

    void Update()
    {
        if (targetPlayer)
        ForScoreTextUI.text = "Player " + targetPlayer.GetIdPlayer() + " Score : " + targetPlayer.GetScore();
    }

    private void OnTriggerEnter(Collider other)
    {
        Target obj = other.gameObject.GetComponent<Target>();
        int tempScore = 0 ;
        if(obj)
        {
            if (other.transform.tag == GameSceneManager.targetPicture && !obj.GetIsCheck())
            {
                other.gameObject.GetComponent<Target>().SetIsCheck(true);
                tempScore += 1;
                targetPlayer.SetScore(tempScore);
            }
            else
            {
                if (targetPlayer.GetScore() > 0)
                {
                    targetPlayer.SetScore(tempScore -= 1);
                }
            }

            obj.SetDestroy(true);
        }
    }
}
