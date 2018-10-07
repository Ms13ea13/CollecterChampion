﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Target obj = other.gameObject.GetComponent<Target>();

        if(obj)
        {
            if (other.transform.tag == GameManagerStage1._targetPicture && !obj.GetIsCheck())
            {
                other.gameObject.GetComponent<Target>().SetIsCheck(true);
                GameManagerStage1._score += 1;
            }
            else
            {
                if (GameManagerStage1._score>0)
                    GameManagerStage1._score -= 1;
            }
            
             obj.SetDestroy(true);
          
            
        }
    }
}
