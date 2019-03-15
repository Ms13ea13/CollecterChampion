using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToCamera2 : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(new Vector3(0, -90, -90));
    }
}
