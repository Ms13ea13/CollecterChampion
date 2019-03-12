using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScale : MonoBehaviour
{
    void Start()
    {
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }
}
