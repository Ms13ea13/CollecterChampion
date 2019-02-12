using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToCook : MonoBehaviour
{
    [SerializeField]
    private GameObject howToCook;

    void Start()
    {
        Time.timeScale = 0;
        howToCook.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Time.timeScale = 1;
            howToCook.SetActive(false);
        }
    }
}
