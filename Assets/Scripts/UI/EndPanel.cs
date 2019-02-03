using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private AudioSource[] stopSound;

    //public GameObject[] disableOBJ;

    void Update()
    {
        /*disableOBJ[0].SetActive(false);
        disableOBJ[1].SetActive(false);*/

        stopSound[0].Stop();
        stopSound[1].Stop();
    }
}
