﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MoveCameraToMouse : MonoBehaviour
{
    float speed = 5.0f;
    public Vector2 limitMax;
    public Vector2 limitMin;
   
	void Update ()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;

		if (Input.GetAxis("Mouse Y") > 0 && transform.position.y < limitMax.y)
        { 
            transform.position += new Vector3(0.0f,Input.GetAxisRaw("Mouse Y")*Time.deltaTime*speed,0);
        }

        if (Input.GetAxis("Mouse Y") < 0 && transform.position.y > limitMin.y)
        {
            transform.position += new Vector3(0.0f,Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed, 0.0f);
        }
    }
}