using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderCheck : MonoBehaviour
{
    [SerializeField]
    private PlayerBase targetPlayer;
    
    private void OnTriggerEnter(Collider other)
    {
        Target obj = other.gameObject.GetComponent<Target>();
        int tempScore = 0 ;
        if(obj)
        {
            if (other.transform.tag == GameManagerStage1._targetPicture && !obj.GetIsCheck())
            {
                other.gameObject.GetComponent<Target>().SetIsCheck(true);
                tempScore += 1;
                targetPlayer.SetScore(tempScore);
            }
            else
            {
                if (targetPlayer.GetScore() > 0)
                {
                    targetPlayer.SetScore(tempScore -=1);
                }
            }

            obj.SetDestroy(true);
        }
    }
}
