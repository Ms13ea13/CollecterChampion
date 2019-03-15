using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private AudioSource[] stopSound;

    //public GameObject[] disableOBJ;

    private bool stop = false;
    void Update()
    {
        /*disableOBJ[0].SetActive(false);
        disableOBJ[1].SetActive(false);*/

        if (stopSound.Length > 0 && stop == false )
        {
            stopSound[0].Stop();
            stopSound[1].Stop();
            stop = true;
        }
    
    }
}
