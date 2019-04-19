﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : MonoBehaviour
{
    public GameObject[] wayPoints;
    int current = 0;
    float rotSpeed;
    public float speed;
    float WPradius = 0.1f;
	
	void Update ()
    {
        if (Vector3.Distance(wayPoints[current].transform.position, transform.position) < WPradius)
        {
            current++;
            //gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y + 90, 0);

            if (current >= wayPoints.Length)
            {
                current = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, wayPoints[current].transform.position, Time.deltaTime * speed);
    }
}